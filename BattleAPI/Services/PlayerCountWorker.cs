using Battlelog;
using CompanionAPI.Helpers;
using CompanionAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimescaleDAL;

namespace BattleAPI.Services
{
    public class PlayerCountWorker : BackgroundService
    {
        private readonly ILogger<PlayerCountWorker> _logger;
        private readonly ICompanionService _companionService;
        private readonly IDistributedCache _distributedCache;
        private readonly PlayerCountsDataBatchProcessor _playerCountsDataBatchProcessor;
        ConcurrentQueue<BlockingCollection<PlayerCountsData>> _queue;

        private readonly string _serverGuid;
        private const int _fetchDelay = 10 * 1000;
        private const int _chunkSize = 3;
        private const int _writeDelay = 15 * 1000;

        public PlayerCountWorker(ILogger<PlayerCountWorker> logger, ICompanionService companionService, IDistributedCache distributedCache)
        {
            _logger = logger;
            _companionService = companionService;
            _distributedCache = distributedCache;
            _serverGuid = Variables.BF_SERVER_GUID;
            _playerCountsDataBatchProcessor = new PlayerCountsDataBatchProcessor(Variables.TIMESCALE_CONNECTION_STRING);
            _queue = new ConcurrentQueue<BlockingCollection<PlayerCountsData>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(_serverGuid))
            {
                throw new Exception("Missing the server guid.");
            }

            if (string.IsNullOrWhiteSpace(Variables.TIMESCALE_CONNECTION_STRING))
            {
                throw new Exception("Missing the timescale db connection string.");
            }

            try
            {
                if (!_playerCountsDataBatchProcessor.TestConnection())
                {
                    throw new Exception("Unable to connect to timescale database.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Postgres database connection failed.");
                return;
            }

            Task fetcher = PlayerCountFetcher(stoppingToken);
            Task writer = PlayerCountWriter(stoppingToken);
            await Task.WhenAll(fetcher, writer);
        }

        private async Task PlayerCountFetcher(CancellationToken stoppingToken)
        {
            BlockingCollection<PlayerCountsData> blockingList = new BlockingCollection<PlayerCountsData>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    FetchCompanionCounts(out var companion);
                    FetchBattlelogCounts(out var battlelog);
                    FetchSnapshotCounts(out var snapshot);
                    var data = companion.CompanionPlayerCountsToCombined(battlelog, snapshot);
                    blockingList.Add(new PlayerCountsData(_serverGuid, data));

                    if (blockingList.Count >= _chunkSize)
                    {
                        blockingList.CompleteAdding();
                        _queue.Enqueue(blockingList);
                        blockingList = new BlockingCollection<PlayerCountsData>();
                    }

                    //_logger.LogTrace($"{{time}} | Queue: {_currentData?.Snapshot?.WaitingPlayers,2} - Players: {_currentData?.Snapshot?.GetTotalPlayers(),2} - Joinig: {_currentData?.Snapshot?.GetJoiningPlayers(),2}", DateTimeOffset.Now);
                    _logger.LogTrace($"{{time}} | Fetched new data", DateTimeOffset.Now);
                    await Task.Delay(_fetchDelay, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(PlayerCountFetcher)} running at: {{time}}", DateTimeOffset.Now);
                    await Task.Delay(_fetchDelay, stoppingToken);
                }
            }
        }

        private async Task PlayerCountWriter(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryDequeue(out var playerCountsDatas))
                    {
                        _playerCountsDataBatchProcessor.WriteToDatabase(playerCountsDatas.ToArray());

                        _logger.LogTrace($"{{time}} | Wrote new data", DateTimeOffset.Now);
                        await Task.Delay(_writeDelay, stoppingToken);
                        continue;
                    }

                    _logger.LogTrace($"{{time}} | Failed to dequeue", DateTimeOffset.Now);
                    await Task.Delay(_writeDelay, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{nameof(PlayerCountWriter)} running at: {{time}}", DateTimeOffset.Now);
                    await Task.Delay(_writeDelay, stoppingToken);
                }
            }
        }

        private bool FetchBattlelogCounts(out BattlelogPlayerCountsViewModel model)
        {
            model = null;
            try
            {
                model = BattlelogClient.GetNumPlayersOnServer(_serverGuid);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server slots for guid {guid}", _serverGuid);
                return false;
            }
        }

        private bool FetchCompanionCounts(out ServerDetailsViewModel model)
        {
            model = null;
            try
            {
                var cacheKey = _serverGuid;
                var gameId = GetGameIdFromCache(cacheKey);

            fetchNewGameId:
                try
                {
                    if (!string.IsNullOrEmpty(gameId))
                    {
                        _logger?.LogInformation("GameId {gameId} fetched from cache for Guid {guid}", gameId, _serverGuid);

                        _logger?.LogInformation("Retrieving server slots for gameId {gameId}", gameId);
                        model = _companionService?.GetServerDetails(gameId);

                        // Check if guid matches (in other words whether the gameId was correct or not)
                        if (!_serverGuid.Equals(model?.Guid, StringComparison.OrdinalIgnoreCase))
                        {
                            gameId = null;
                            goto fetchNewGameId;
                        }
                    }
                    else
                    {
                        var serverInfo = BattlelogClient.GetServerShow(_serverGuid, "pc");
                        gameId = serverInfo.gameId;

                        _distributedCache.SetStringAsync(cacheKey, gameId).ConfigureAwait(false);
                        _logger?.LogInformation("GameId {gameId} added to cache for Guid {guid}", gameId, _serverGuid);

                        _logger?.LogInformation("Retrieving server slots for gameId {gameId}", gameId);
                        model = _companionService?.GetServerDetails(gameId);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Unable to retrieve companion slots");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server slots for guid {guid}", _serverGuid);
                return false;
            }
        }

        private bool FetchSnapshotCounts(out Snapshot model)
        {
            model = null;
            try
            {
                model = BattlelogClient.GetServerSnapshot(_serverGuid);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server slots for guid {guid}", _serverGuid);
                return false;
            }
        }

        private string GetGameIdFromCache(string cacheKey)
        {
            try
            {
                return _distributedCache.GetString(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while trying to fetch key from Redis.");

                return null;
            }
        }
    }
}
