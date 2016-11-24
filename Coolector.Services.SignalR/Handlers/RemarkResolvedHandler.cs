using System;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Services;
using NLog;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkResolvedHandler : IEventHandler<RemarkResolved>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRemarkSignalRService _signalRService;

        public RemarkResolvedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkResolved @event)
        {
            try
            {
                await _signalRService.PublishRemarkResolvedAsync(@event);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}