using Autofac;
using Coolector.Common.Nancy;
using Microsoft.Extensions.Configuration;

namespace Coolector.Services.Signalr
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        private IConfiguration _configuration;

        public static ILifetimeScope LifetimeScope { get; private set; }

        public Bootstrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}