using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Services;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IRemarkSignalRService _signalRService;

        public RemarkDeletedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            await _signalRService.PublishRemarkDeletedAsync(@event);
        }
    }
}