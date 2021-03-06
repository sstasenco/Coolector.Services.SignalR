﻿using Coolector.Common.Host;
using Coolector.Services.Remarks.Shared.Events;

namespace Coolector.Services.SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost.Create<Startup>(port: 15000)
                .UseAutofac(Startup.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToEvent<RemarkCreated>()
                .SubscribeToEvent<RemarkResolved>()
                .SubscribeToEvent<RemarkDeleted>()
                .Build()
                .Run();
        }
    }
}