using EasyWater.Service.Core.Configurations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace EasyWater.Service.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDatabaseProvider(this IServiceCollection services)
        {
            Func<IServiceProvider, IFreeSql> implementationFreeSql = r =>
            {
                IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.SqlServer, Configuration.DbConnectionString)
                .CreateDatabaseIfNotExists()
                .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))
                //.UseAutoSyncStructure(true)
                //Automatically synchronize the entity structure to the database.
                .Build();
                return fsql;
            };
            services.AddSingleton<IFreeSql>(implementationFreeSql);
            services.AddMigrationConfigurationService();
        }
        
        private static void AddMigrationConfigurationService(this IServiceCollection services)
        {
            services
                .AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c
                .AddSqlServer2012()
                .WithGlobalConnectionString(Configuration.DbConnectionString)
                .ScanIn(Assembly.GetExecutingAssembly()).For.All());
        }      
    }
}
