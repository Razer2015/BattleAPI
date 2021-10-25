using BattleAPI.Filters;
using BattleAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Enums;
using Shared.Models;
using System;

namespace BattleAPI.Controllers.V1ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [PersonaAuth]
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPersonaService _personaService;

        public UserController(ILogger<UserController> logger, IPersonaService personaService)
        {
            _logger = logger;
            _personaService = personaService;
        }

        [HttpGet("getUsersByPersonaNames/")]
        public IActionResult GetUsersByPersonaNames([FromQuery] string[] personaNames, [FromQuery] DashKind kind)
        {
            try
            {
                var response = Battlelog.BattlelogClient.GetUsersByPersonaNames(personaNames, kind);

                if (response == null)
                {
                    _logger?.LogError("Failed to retrieve users by persona names {personaNames} - {kind}", personaNames, kind);
                    return BadRequestBattlelogResponse<PersonaInfo>(null, "Unknown error while fetching users by persona names");
                }
                
                _logger?.LogInformation("Retrieved users by persona names {personaNames} - {kind}", personaNames, kind);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve users by persona names {personaNames} - {kind}", personaNames, kind);
                return null;
            }
        }

        [HttpGet("getUsersByPersonaIds/")]
        public IActionResult GetUsersByPersonaIds([FromQuery] string[] personaIds, [FromQuery] DashKind kind)
        {
            try
            {
                var response = Battlelog.BattlelogClient.GetUsersByPersonaIds(personaIds, kind);

                if (response == null)
                {
                    _logger?.LogError("Failed to retrieve users by persona ids {personaIds} - {kind}", personaIds, kind);
                    return BadRequestBattlelogResponse<PersonaInfo>(null, "Unknown error while fetching users by persona ids");
                }

                _logger?.LogInformation("Retrieved users by persona ids {personaIds} - {kind}", personaIds, kind);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve users by persona ids {personaIds} - {kind}", personaIds, kind);
                return null;
            }
        }

        [HttpGet("getUsersByIds/")]
        public IActionResult GetUsersByIds([FromQuery] string[] userIds, [FromQuery] DashKind kind)
        {
            try
            {
                var response = Battlelog.BattlelogClient.GetUsersByUserIds(userIds, kind);

                if (response == null)
                {
                    _logger?.LogError("Failed to retrieve users by user ids {userIds} - {kind}", userIds, kind);
                    return BadRequestBattlelogResponse<PersonaInfo>(null, "Unknown error while fetching users by user ids");
                }

                _logger?.LogInformation("Retrieved users by user ids {userIds} - {kind}", userIds, kind);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve users by user ids {userIds} - {kind}", userIds, kind);
                return null;
            }
        }
    }
}
