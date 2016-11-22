using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NLog;

namespace Coolector.Services.Signalr.Hubs
{
    public class RemarksHub : Hub
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override Task OnConnectedAsync()
        {
            Logger.Debug($"Connetcted to Hub, connectionId:{Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync()
        {
            Logger.Debug($"Disconnected from hub, connectionId:{Context.ConnectionId}");
            return base.OnDisconnectedAsync();
        }
    }
}