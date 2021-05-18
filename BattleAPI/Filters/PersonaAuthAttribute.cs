using Microsoft.AspNetCore.Mvc.Filters;
using Shared;
using System;
using System.Threading.Tasks;

namespace BattleAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PersonaAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey) ||
                context.HttpContext.Request.Query.TryGetValue(ApiKeyHeaderName, out potentialApiKey))
                {
                    var apiKey = Variables.PERSONA_API_KEY;
                    if (!string.IsNullOrWhiteSpace(apiKey) && apiKey.Equals(potentialApiKey))
                    {
                        context.HttpContext.Items.Add(Consts.IsAuthorized, "true");
                    }
                }
            }
            catch {
                // Suppress
            }

            await next();
        }
    }
}
