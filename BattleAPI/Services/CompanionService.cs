using CompanionAPI;
using CompanionAPI.Models;
using Origin.Authentication;
using Shared;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Services;
using System;

namespace BattleAPI.Services
{
    public interface ICompanionService
    {
        ServerDetailsViewModel GetServerDetails(string gameId, string game = "bf4");
    }

    public class CompanionService : ICompanionService
    {
        private readonly ILoggingService _loggingService;
        private readonly IAuthCodeService _authCodeService;

        private CompanionClient _companionClient;

        public CompanionService(ILoggingService loggingService, IAuthCodeService authCodeService)
        {
            _loggingService = loggingService;
            _authCodeService = authCodeService;

            Login();
        }

        public ServerDetailsViewModel GetServerDetails(string gameId, string game = "bf4")
        {
            OutputModel<ServerDetailsViewModel> output = null;
            try
            {
                if (!_companionClient?.GetServerDetails(game, gameId, out output) ?? false || output.Response.Status != Status.Success)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.ToString(), LogType.Error);
                return null;
            }

            return output.Model;
        }

        private void Login()
        {
            try
            {
                var email = Variables.EA_EMAIL;
                var password = Variables.EA_PASSWORD;

                // Get authentication token
                var auth = new Auth(_authCodeService);
                var loginType = auth.Login(email, password);

                // Create companion client
                var companion = new CompanionClient(auth);
                if (companion.Login(out var responseStatus))
                {
                    _companionClient = companion;
                    LogMessage($"Logged in to: {auth.GetEmail} with {loginType}", LogType.Information);
                }
                else
                {
                    LogMessage($"{responseStatus?.Status}: Login failed - {responseStatus?.Message}", LogType.Error);
                }
            }
            catch (Exception e)
            {
                LogMessage(e.ToString(), LogType.Error);
            }
        }

        private void LogMessage(string message, LogType logType)
        {
            try
            {
                var timeStamp = DateTime.Now;
                switch (logType)
                {
                    case LogType.Information:
                        Console.WriteLine($"{DateTime.Now} - Information: {message}");
                        break;
                    case LogType.Warning:
                        Console.WriteLine($"{DateTime.Now} - Warning: {message}");
                        break;
                    case LogType.Error:
                        Console.WriteLine($"{DateTime.Now} - Error: {message}");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logType));
                }
            }
            catch (Exception e)
            {
                _loggingService.WriteException(e);
            }
        }
    }
}
