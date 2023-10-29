using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using EasyWater.Service.Bootstrap;
using EasyWater.Service.Core.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EasyWater.Service.Startup))]

namespace EasyWater.Service
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDatabaseProvider();

            builder.UseAutofacServiceProviderFactory(ConfigureContainer);

            //builder.Services.AddSingleton(GetContainer(builder.Services));

            //// Important: Use AddScoped so our Autofac lifetime scope gets disposed
            //// when the function finishes executing
            //builder.Services.AddScoped<LifetimeScopeWrapper>();

            //builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivator), typeof(AutofacJobActivator)));
            //builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivatorEx), typeof(AutofacJobActivator)));
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // this is optional and will bind IConfiguration with appsettings.json in
            // the container, like it is usually done in regular dotnet console and
            // web applications.
            builder.UseAppSettings();
        }

        private IContainer ConfigureContainer(ContainerBuilder builder)
        {
            var currentAssembly = typeof(Startup).Assembly;
            // Register all functions that resides in a given namespace
            // The function class itself will be created using autofac
            builder
                .RegisterAssemblyTypes(currentAssembly)
                .InNamespace("EasyWater.Service.Functions")
                .AsSelf() // Azure Functions core code resolves a function class by itself.
                .InstancePerTriggerRequest(); // This will scope nested dependencies to each function execution

            builder
                .AddMigrationService()
                .AddEasyWaterServices();

            var appContainer = builder.Build();

            // If you need a Multi-Tenant Container, use this code instead of plain appContainer;
            // var multiTenant = new MultitenantContainer(tenantIdentificationStrategy, appContainer);
            // return multiTenant

            return appContainer;
        }

        private static IContainer GetContainer(IServiceCollection serviceCollection)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(serviceCollection);
            containerBuilder.RegisterModule<LoggerModule>();

            // This is a convenient way to register all your function classes at once
            containerBuilder.RegisterAssemblyTypes(typeof(Startup).Assembly)
                .InNamespaceOf<Functions.FunctionMigration>();

            // TODO: Register other dependencies with the ContainerBuilder like normal

            return containerBuilder.Build();
        }
    }
}
