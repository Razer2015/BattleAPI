using CompanionAPI.Battlelog;
using CompanionAPI.Battlelog.Models;
using CompanionAPI.Companion.Models;
using CompanionAPI.Models;
using Newtonsoft.Json;
using Origin.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace CompanionAPI
{
    public delegate V CallDelegate<T, U, V>(T input, out U output);

    public class CompanionClient
    {
        private string _session;
        private string _clientVersion = "companion-init";

        private readonly Auth _originAuth;

        public CompanionClient(Auth originAuth)
        {
            _originAuth = originAuth;
        }

        public bool Login(out ResponseStatus status)
        {
            //if (!_originAuth.IsAuthenticated) {
            //    _originAuth.ReLogin();
            //}

            var method = "Companion.loginFromAuthCode";
            var retryCount = 2;
            Response<LoginViewModel> response = null;
            for (int i = 0; i <= retryCount; i++)
            {
                if (i > 0)
                {
                    _originAuth.ReLogin();
                }
                if (PostRequest<LoginViewModel>(method, new RequestParams { Code = _originAuth.CompanionToken, RedirectUri = "nucleus:rest" }, out response))
                {
                    status = response.ResponseStatus;
                    _session = response.Result.Id;
                    _clientVersion = "companion-9014390";
                    return true;
                }
                if (i > 0)
                {
                    // Wait 2 seconds before trying again
                    Thread.Sleep(2 * 1000);
                }
            }
            status = response.ResponseStatus;
            return false;
        }

        public string GetEmail { get { return _originAuth.GetEmail; } }

        private bool ExcecuteMethod<T>(CallDelegate<RequestParams, OutputModel<T>, bool> method, RequestParams @params, out OutputModel<T> outputModel)
        {
            outputModel = null;

            for (int i = 0; i < 2; i++)
            {
                if (method(@params, out outputModel))
                {
                    return true;
                }
                else
                {
                    if (outputModel.Response.Status == Status.InvalidSession)
                    {
                        // Login and retrieve session token
                        if (!Login(out var response))
                        {
                            throw new Exception($"Invalid session and couldn't refresh. {response.Message}");
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown error.");
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///     Reserve a slot from the server
        /// </summary>
        /// <param name="game">e.g. bf4</param>
        /// <param name="gameId">e.g. 18014398527195660</param>
        /// <param name="role">e.g. soldier or spectator</param>
        /// <returns></returns>
        public bool ReserveSlot(string game, string gameId, string role, out OutputModel<GameJoinViewModel> outputModel)
        {
            return ExcecuteMethod(ReserveSlot, new RequestParams { Game = game, GameId = gameId, Settings = new Settings { Role = role } }, out outputModel);
        }

        private bool ReserveSlot(RequestParams requestParams, out OutputModel<GameJoinViewModel> outputModel)
        {
            outputModel = new OutputModel<GameJoinViewModel>();

            var method = "Game.reserveSlot";
            if (PostRequest<GameJoinViewModel>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }

        /// <summary>
        ///     Leave the game server
        /// </summary>
        /// <param name="game">e.g. bf4</param>
        /// <param name="gameId">e.g. 18014398527195660</param>
        /// <returns></returns>
        public bool LeaveGame(string game, string gameId, out OutputModel<string> outputModel)
        {
            return ExcecuteMethod(LeaveGame, new RequestParams { Game = game, GameId = gameId }, out outputModel);
        }

        private bool LeaveGame(RequestParams requestParams, out OutputModel<string> outputModel)
        {
            outputModel = new OutputModel<string>();

            var method = "Game.leaveGame";
            if (PostRequest<string>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }

        /// <summary>
        /// Get persona info as career
        /// </summary>
        /// <param name="token"></param>
        /// <param name="personaId"></param>
        /// <returns></returns>
        public StatsViewModel GetPersonaInfo(string personaId)
        {
            ExcecuteMethod<CareerViewModel>(GetCareer, new RequestParams { PersonaId = personaId }, out var output);
            return output.Model;
        }

        /// <summary>
        /// Get all persona info
        /// </summary>
        /// <param name="token"></param>
        /// <param name="personaId"></param>
        /// <param name="game"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public StatsViewModel GetPersonaInfo(string personaId, string game, string platform)
        {
            // Get detailed stats
            ExcecuteMethod<DetailedStatsViewModel>(GetDetailedStats, new RequestParams { Game = game, PersonaId = personaId }, out var detailedStatsOutput);

            // Get emblem
            ExcecuteMethod<string>(GetEquippedEmblem, new RequestParams { PersonaId = personaId, Platform = platform }, out var emblemOutput);
            detailedStatsOutput.Model.EmblemUrl = emblemOutput.Model;

            return detailedStatsOutput.Model;
        }

        #region Detailed Stats
        /// <summary>
        /// Get detailed stats of a game for person
        /// </summary>
        /// <param name="game">BF4 = bf4, BF1 = tunguska, BFV = casablanca</param>
        /// <param name="personaId">Unique Id used to identify a player</param>
        public bool GetDetailedStats(string game, string personaId, out OutputModel<DetailedStatsViewModel> outputModel)
        {
            return ExcecuteMethod(GetDetailedStats, new RequestParams { Game = game, PersonaId = personaId }, out outputModel);
        }

        /// <summary>
        /// Get detailed stats of a game for person
        /// </summary>
        private bool GetDetailedStats(RequestParams requestParams, out OutputModel<DetailedStatsViewModel> outputModel)
        {
            outputModel = new OutputModel<DetailedStatsViewModel>();

            var method = "Stats.detailedStatsByPersonaId";
            if (PostRequest<DetailedStatsViewModel>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }
        #endregion

        #region Emblem
        /// <summary>
        /// Get the equipped emblem url of the persona
        /// </summary>
        public bool GetEquippedEmblem(string personaId, string platform, out OutputModel<string> outputModel)
        {
            return ExcecuteMethod(GetEquippedEmblem, new RequestParams { PersonaId = personaId, Platform = platform }, out outputModel);
        }

        /// <summary>
        /// Get the equipped emblem url of the persona
        /// </summary>
        private bool GetEquippedEmblem(RequestParams requestParams, out OutputModel<string> outputModel)
        {
            outputModel = new OutputModel<string>();

            var method = "Emblems.getEquippedEmblem";
            requestParams.Platform = (string.IsNullOrEmpty(requestParams.Platform) ? "pc" : requestParams.Platform);
            if (PostRequest<string>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }
        #endregion

        #region Career
        public bool GetCareer(string personaId, out OutputModel<CareerViewModel> outputModel)
        {
            return ExcecuteMethod(GetCareer, new RequestParams { PersonaId = personaId }, out outputModel);
        }

        private bool GetCareer(RequestParams requestParams, out OutputModel<CareerViewModel> outputModel)
        {
            outputModel = new OutputModel<CareerViewModel>();

            var method = "Stats.getCareerForOwnedGamesByPersonaId";
            if (PostRequest<CareerViewModel>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }
        #endregion

        #region GameServer
        public bool GetServerDetails(string game, string gameId, out OutputModel<ServerDetailsViewModel> outputModel)
        {
            return ExcecuteMethod(GetServerDetails, new RequestParams { Game = game, GameId = gameId }, out outputModel);
        }

        private bool GetServerDetails(RequestParams requestParams, out OutputModel<ServerDetailsViewModel> outputModel)
        {
            outputModel = new OutputModel<ServerDetailsViewModel>();

            var method = "GameServer.getServerDetails";
            if (PostRequest<ServerDetailsViewModel>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }
        #endregion

        #region GetWeaponStats
        public bool GetWeaponStats(string game, string personaId, out OutputModel<List<WeaponStatCategory>> outputModel)
        {
            return ExcecuteMethod(GetWeaponStats, new RequestParams { Game = game, PersonaId = personaId }, out outputModel);
        }

        public bool GetBattlelogWeaponStats(string game, string personaId, out BattlelogResponse<WeaponStats> outputModel)
        {
            try
            {
                var result = ExcecuteMethod<List<WeaponStatCategory>>(GetWeaponStats, new RequestParams { Game = game, PersonaId = personaId }, out var output);
                //var result = GetWeaponStats(new RequestParams { Game = game, PersonaId = personaId }, out var output);

                var mainWeaponStats = output.Model
                    .SelectMany(x => x.Weapons)
                    .Select(x => {
                        var weaponInfo = WeaponConverter.WeaponInfo(x.Guid);

                        return new MainWeaponStat {
                            Guid = x.Guid,
                            ServiceStars = (int)x.Stats.Values.Kills / 100,
                            ServiceStarsProgress = x.Stats.Values.Kills % 100,
                            Category = weaponInfo?.Category,
                            CategorySID = weaponInfo?.CategorySID,
                            Slug = weaponInfo?.Slug,
                            Name = weaponInfo?.Name,
                            Kills = x.Stats.Values.Kills,
                            ShotsFired = x.Stats.Values.Shots,
                            ShotsHit = x.Stats.Values.Hits,
                            Accuracy = x.Stats.Values.Accuracy / 100,
                            Headshots = x.Stats.Values.Headshots,
                            TimeEquipped = x.Stats.Values.Seconds
                        };
                    })
                    .Where(x => !string.IsNullOrEmpty(x.Name))
                    .ToList();

                outputModel = new BattlelogResponse<WeaponStats> {
                    Type = "success",
                    Message = "OK",
                    Data = new WeaponStats {
                        PersonaId = ulong.Parse(personaId),
                        MainWeaponStats = mainWeaponStats
                    }
                };

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                outputModel = new BattlelogResponse<WeaponStats> {
                    Type = "error",
                    Message = "FAIL"
                };

                return false;
            }
        }

        private bool GetWeaponStats(RequestParams requestParams, out OutputModel<List<WeaponStatCategory>> outputModel)
        {
            outputModel = new OutputModel<List<WeaponStatCategory>>();

            var method = "Progression.getWeaponsByPersonaId";
            if (PostRequest<List<WeaponStatCategory>>(method, requestParams, out var result))
            {
                outputModel.Response = result.ResponseStatus;
                outputModel.Model = result.Result;
                return true;
            }
            else
            {
                outputModel.Response = result.ResponseStatus;
                return false;
            }
        }
        #endregion

        #region Request
        private static string GenerateRequestData(string method, RequestParams data)
        {
            return JsonConvert.SerializeObject(new Request<RequestParams>(method, data),
                Formatting.None,
                new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        private bool PostRequest<T>(string method, RequestParams @params, out Response<T> result)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create($"{Constants.CompanionAPI}{method}");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; BattleAPI)";
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;

            //Get the headers associated with the request.
            WebHeaderCollection myWebHeaderCollection = httpWebRequest.Headers;
            myWebHeaderCollection.Add("X-ClientVersion: " + _clientVersion);
            if (!string.IsNullOrEmpty(_session))
            {
                myWebHeaderCollection.Add("X-GatewaySession: " + _session);
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(GenerateRequestData(method, @params));
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var stream = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<Response<T>>(stream);
                    return true;
                }
            }
            catch (WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var stream = streamReader.ReadToEnd();
                    result = JsonConvert.DeserializeObject<Response<T>>(stream);
                }

                return false;
            }
        }
        #endregion
    }
}
