using BattleAPI.Filters;
using BattleAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Models;
using System;

namespace BattleAPI.Controllers.V1ApiControllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [PersonaAuth]
    public class PersonaController : BaseController
    {
        private readonly ILogger<PersonaController> _logger;
        private readonly IPersonaService _personaService;

        public PersonaController(ILogger<PersonaController> logger, IPersonaService personaService)
        {
            _logger = logger;
            _personaService = personaService;
        }

        [HttpGet("personaByName/{soldierName}/{serverGuid?}/")]
        public IActionResult PersonaByName(string soldierName, string serverGuid)
        {
            try
            {
                var model = _personaService.GetPersona(null, soldierName, serverGuid);

                if (model == null)
                {
                    _logger?.LogError("Failed to retrieve persona for soldier {soldierName} in server {serverGuid}", soldierName, serverGuid);
                    return BadRequestBattlelogResponse<PersonaInfo>(null, "Couldn't retrieve persona info");
                }
                
                _logger?.LogInformation("Retrieved persona for soldier {soldierName} in server {serverGuid}", soldierName, serverGuid);
                return SuccessBattlelogResponse(model);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve persona info for soldier: {soldierName} - server: {serverGuid}", soldierName, serverGuid);
                return null;
            }
        }

        [HttpGet("persona/{eaGuid}/{soldierName}/{serverGuid?}/")]
        public IActionResult PersonaInfo(string eaGuid, string soldierName, string serverGuid)
        {
            try
            {
                var model = _personaService.GetPersona(eaGuid, soldierName, serverGuid, HttpContext.Items.ContainsKey(Consts.IsAuthorized));

                if (model == null)
                {
                    _logger?.LogError("Failed to retrieve persona for guid {eaGuid} - soldier {soldierName} in server {serverGuid}", eaGuid, soldierName, serverGuid);
                    return BadRequestBattlelogResponse<PersonaInfo>(null, "Couldn't retrieve persona info");
                }

                _logger?.LogInformation("Retrieved persona for guid {eaGuid} - soldier {soldierName} in server {serverGuid}", eaGuid, soldierName, serverGuid);
                return SuccessBattlelogResponse(model);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Couldn't retrieve persona info for eaGuid: {eaGuid} - soldier: {soldierName} - server: {serverGuid}", eaGuid, soldierName, serverGuid);
                return null;
            }
        }
    }
}
