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
        private const string ENV_ENABLE_PLAYERCOUNT_LOGGING = "ENABLE_PLAYERCOUNT_LOGGING";

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

        /// <summary>
        ///     Whether the player count logging should be enabled or not
        /// </summary>
        public static bool PLAYERCOUNT_LOGGING_ENABLED
        {
            get {
                _ = bool.TryParse(Environment.GetEnvironmentVariable(ENV_ENABLE_PLAYERCOUNT_LOGGING), out var enabled);
                return enabled;
            }
        }

        /// <summary>
        ///     Get the server GUID
        /// </summary>
        public static string BF_SERVER_GUID { get { return Environment.GetEnvironmentVariable("SERVER_GUID"); } }

        /// <summary>
        ///     Get the timescale db connection string
        /// </summary>
        public static string TIMESCALE_CONNECTION_STRING { get { return Environment.GetEnvironmentVariable("TIMESCALE_CONNECTION_STRING"); } }
    }
}
