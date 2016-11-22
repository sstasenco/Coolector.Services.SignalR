using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IHubContext<RemarksHub> _hubContext;

        public RemarkResolvedHandler(IHubContext<RemarksHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _hubContext.Clients.All.InvokeAsync("RemarkResolved", @event);
        }
    }
}