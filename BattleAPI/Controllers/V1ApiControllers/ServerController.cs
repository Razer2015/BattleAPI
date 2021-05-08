using BattleAPI.Services;
using Battlelog;
using CompanionAPI.Battlelog.Models;
using CompanionAPI.Helpers;
using CompanionAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Shared.Models;
using System;

namespace BattleAPI.Controllers.V1ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServerController : BaseController
    {
        private readonly ILogger<ServerController> _logger;
        private readonly ICompanionService _companionService;
        private readonly IDistributedCache _distributedCache;

        public ServerController(ILogger<ServerController> logger, ICompanionService companionService, IDistributedCache distributedCache)
        {
            _logger = logger;
            _companionService = companionService;
            _distributedCache = distributedCache;
        }

        [HttpGet("getNumPlayersOnServer/{platform}/{guid}/{type?}/")]
        public IActionResult GetNumPlayersOnServer(string platform, string guid, SlotResponseType type = SlotResponseType.Battlelog)
        {
            try
            {
                var cacheKey = guid;
                var gameId = GetGameIdFromCache(cacheKey);
                if (!string.IsNullOrEmpty(gameId))
                {
                    _logger?.LogInformation("GameId {gameId} fetched from cache for Guid {guid}", gameId, guid);
                }
                else
                {
                    var serverInfo = BattlelogClient.GetServerShow(guid, platform);
                    gameId = serverInfo.gameId;

                    _distributedCache.SetStringAsync(cacheKey, gameId, new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    }).ConfigureAwait(false);
                    _logger?.LogInformation("GameId {gameId} added to cache for Guid {guid}", gameId, guid);
                }

                _logger?.LogInformation("Retrieving server slots for gameId {gameId}", gameId);
                var model = _companionService?.GetServerDetails(gameId);

                if (model?.Slots == null)
                {
                    return BadRequestBattlelogResponse<SlotTypesViewModel>(null, "Couldn't retrieve server slots");
                }

                return type switch {
                    SlotResponseType.Companion => SuccessBattlelogResponse(model.Slots),
                    _ => Ok(model.CompanionPlayerCountsToBattlelog()),
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server slots for guid {guid}", guid);
                return null;
            }
        }

        [HttpGet("serverSlots/{gameId}/{type?}/")]
        public IActionResult ServerSlotsByGameId(string gameId, SlotResponseType type = SlotResponseType.Battlelog)
        {
            try
            {
                _logger?.LogInformation("Retrieving server slots for gameId {gameId}", gameId);

                var model = _companionService?.GetServerDetails(gameId);

                if (model?.Slots == null)
                {
                    return BadRequestBattlelogResponse<SlotTypesViewModel>(null, "Couldn't retrieve server slots");
                }

                return type switch {
                    SlotResponseType.Companion => SuccessBattlelogResponse(model.Slots),
                    _ => Ok(model.CompanionPlayerCountsToBattlelog()),
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server slots for gameId {gameId}", gameId);
                return null;
            }
        }

        [HttpGet("serverDetails/{gameId}/")]
        public IActionResult ServerDetailsByGameId(string gameId)
        {
            try
            {
                _logger?.LogInformation("Retrieving server details for gameId {gameId}", gameId);

                var model = _companionService?.GetServerDetails(gameId);

                if (model == null)
                {
                    return BadRequestBattlelogResponse<ServerDetailsViewModel>(null, "Couldn't retrieve server details");
                }
                
                return SuccessBattlelogResponse(model);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve server details for gameId {gameId}", gameId);
                return null;
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
