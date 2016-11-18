using Coolector.Common.Host;

namespace Coolector.Services.Signalr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost.Create<Startup>(port: 15000)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .Build()
                .Run();
        }
    }
}