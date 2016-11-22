using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Services;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private readonly IRemarkSignalRService _signalRService;

        public RemarkResolvedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            await _signalRService.PublishRemarkResolvedAsync(@event);
        }
    }
}