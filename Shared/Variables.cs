using System;

namespace Shared
{
    public static class Variables
    {
        public static string PERSONA_API_KEY = Environment.GetEnvironmentVariable("PERSONA_API_KEY");
        public static string EA_EMAIL = Environment.GetEnvironmentVariable("EA_EMAIL");
        public static string EA_PASSWORD = Environment.GetEnvironmentVariable("EA_PASSWORD");
        public static string REDIS_CONFIGURATION = Environment.GetEnvironmentVariable("REDIS_CONFIGURATION");
        public static string REDIS_INSTANCE = Environment.GetEnvironmentVariable("REDIS_INSTANCE");

        private const string ENV_ENABLE_SWAGGER = "ENABLE_SWAGGER";

        /// <summary>
        ///     Whether the swagger should be enabled or not
        /// </summary>
        public static bool SWAGGER_ENABLED
        {
            get {
                _ = bool.TryParse(Environment.GetEnvironmentVariable(ENV_ENABLE_SWAGGER), out var swaggerEnabled);
                return swaggerEnabled;
            }
        }
    }
}
