using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using EasyWater.Service.Core.Services;

namespace EasyWater.Service.Core.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddMigrationService(this ContainerBuilder builder)
        {
            builder
               .RegisterType<MigrationService>()
               .AsImplementedInterfaces()
               .InstancePerTriggerRequest();

            return builder;
        }
    }
}
