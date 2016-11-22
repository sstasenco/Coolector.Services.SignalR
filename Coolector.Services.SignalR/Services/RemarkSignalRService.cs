using System.Threading.Tasks;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Coolector.Services.SignalR.Services
{
    public class RemarkSignalRService : IRemarkSignalRService
    {
        private readonly IHubContext<RemarksHub> _hubContext;

        public RemarkSignalRService(IHubContext<RemarksHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishRemarkCreatedAsync(RemarkCreated @event)
        {
            var message = new
            {
                RemarkId = @event.RemarkId
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkCreated", message);
        }

        public async Task PublishRemarkResolvedAsync(RemarkResolved @event)
        {
            var message = new
            {
                RemarkId = @event.RemarkId
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkCreated", message);
        }

        public async Task PublishRemarkDeletedAsync(RemarkDeleted @event)
        {
            var message = new
            {
                RemarkId = @event.Id
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkCreated", message);
        }
    }
}