using CompanionAPI.Battlelog.Models;
using Microsoft.AspNetCore.Mvc;

namespace BattleAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        public IActionResult SuccessBattlelogResponse<T>(T data)
        {
            return Ok(new BattlelogResponse<T> {
                Type = "success",
                Message = "OK",
                Data = data
            });
        }

        public IActionResult BadRequestBattlelogResponse<T>(T data, string message)
        {
            return BadRequest(new BattlelogResponse<T> {
                Type = "error",
                Message = message,
                Data = data
            });
        }
    }
}
