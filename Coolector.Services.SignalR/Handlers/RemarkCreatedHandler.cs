using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Services;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IRemarkSignalRService _signalRService;

        public RemarkCreatedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _signalRService.PublishRemarkCreatedAsync(@event);
        }
    }
}