using System;

namespace EasyWater.Service.Core.Configurations
{
    public static class Configuration
    {
        public static string DbConnectionString => Environment.GetEnvironmentVariable("DatabaseConnectionString");
    }
}
