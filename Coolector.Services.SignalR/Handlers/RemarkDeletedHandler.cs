using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IHubContext<RemarksHub> _hubContext;

        public RemarkDeletedHandler(IHubContext<RemarksHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _hubContext.Clients.All.InvokeAsync("RemarkDeleted", @event);
        }
    }
}