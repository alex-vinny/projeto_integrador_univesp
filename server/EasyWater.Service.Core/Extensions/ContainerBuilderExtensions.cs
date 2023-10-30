using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using EasyWater.Service.Core.Services;

namespace EasyWater.Service.Core.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddEasyWaterServices(this ContainerBuilder builder)
        {
            builder
               .RegisterType<AuthService>()
               .As<IAuthService>()
               .AsImplementedInterfaces()
               .InstancePerTriggerRequest();

            builder
              .RegisterType<SensoresService>()
              .As<ISensoresService>()
              .AsImplementedInterfaces()
              .InstancePerTriggerRequest();

            builder
             .RegisterType<ReportService>()
             .As<IReportService>()
             .AsImplementedInterfaces()
             .InstancePerTriggerRequest();

            return builder;
        }
    }
}
