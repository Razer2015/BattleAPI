using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;

namespace BattleAPI.Controllers.V1ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("ping/")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpGet("health/")]
        public async Task<IActionResult> Health()
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();
            return Ok(healthReport);
        }
    }
}
