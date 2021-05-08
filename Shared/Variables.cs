using System;

namespace Shared
{
    public static class Variables
    {
        public static string EA_EMAIL = Environment.GetEnvironmentVariable("EA_EMAIL");
        public static string EA_PASSWORD = Environment.GetEnvironmentVariable("EA_PASSWORD");
        public static string REDIS_CONFIGURATION = Environment.GetEnvironmentVariable("REDIS_CONFIGURATION");
        public static string REDIS_INSTANCE = Environment.GetEnvironmentVariable("REDIS_INSTANCE");
    }
}
