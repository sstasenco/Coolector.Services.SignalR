using System;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.SignalR.Services;
using NLog;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRemarkSignalRService _signalRService;

        public RemarkCreatedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            try
            {
                await _signalRService.PublishRemarkCreatedAsync(@event);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}