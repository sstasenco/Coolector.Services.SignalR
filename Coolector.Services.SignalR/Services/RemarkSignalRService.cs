using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Extensions;
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
            var smallPhoto = @event.Photos.FirstOrDefault(x => x.Size == "small" && x.Metadata.Empty());
            var message = new
            {
                remarkId = @event.RemarkId,
                author = @event.Username,
                category = @event.Category.Name,
                location = new
                {
                    address = @event.Location.Address,
                    coordinates = new[] { @event.Location.Longitude, @event.Location.Latitude },
                    type = "Point"
                },
                smallPhotoUrl = smallPhoto?.Url,
                description = @event.Description,
                createdAt = @event.CreatedAt,
                resolved = false
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkCreated", message);
        }

        public async Task PublishRemarkResolvedAsync(RemarkResolved @event)
        {
            var message = new
            {
                remarkId = @event.RemarkId,
                resolverId = @event.UserId,
                resolver = @event.Username,
                resolvedAt = @event.ResolvedAt
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkResolved", message);
        }

        public async Task PublishRemarkDeletedAsync(RemarkDeleted @event)
        {
            var message = new
            {
                remarkId = @event.Id
            };
            await _hubContext.Clients.All.InvokeAsync("RemarkDeleted", message);
        }
    }
}