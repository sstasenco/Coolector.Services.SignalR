using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Coolector.Common.Commands;
using Coolector.Common.Events;
using Coolector.Common.Extensions;
using Coolector.Services.Signalr.Hubs;
using Lockbox.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace Coolector.Services.Signalr
{
    public class Startup
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public string EnvironmentName { get; }
        public IConfiguration Configuration { get; }
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            EnvironmentName = env.EnvironmentName.ToLowerInvariant();
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .SetBasePath(env.ContentRootPath);

            if (env.IsProduction())
            {
                builder.AddLockbox();
            }

            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddCors();
            services.AddSignalR();

            var rmqRetryPolicy = Policy
                .Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .Or<IOException>()
                .WaitAndRetry(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) => {
                        Logger.Error(exception, $"Cannot connect to RabbitMQ. retryCount:{retryCount}, duration:{timeSpan}");
                    }
                );
            var assembly = Assembly.GetEntryAssembly();
            var builder = new ContainerBuilder();
            var rawRabbitConfiguration = Configuration.GetSettings<RawRabbitConfiguration>();
            rmqRetryPolicy.Execute(() => builder
                    .RegisterInstance(BusClientFactory.CreateDefault(rawRabbitConfiguration))
                    .As<IBusClient>()
            );
            builder.Populate(services);
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            LifetimeScope = builder.Build().BeginLifetimeScope();

            return new AutofacServiceProvider(LifetimeScope);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, IApplicationLifetime appLifeTime)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            app.UseSignalR(builder => builder.MapHub<RemarksHub>("/remarks"));

            appLifeTime.ApplicationStopped.Register(() => LifetimeScope.Dispose());
        }
    }
}