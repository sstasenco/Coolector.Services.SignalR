using System;
using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.SignalR.Services;
using NLog;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IRemarkSignalRService _signalRService;

        public RemarkDeletedHandler(IRemarkSignalRService signalRService)
        {
            _signalRService = signalRService;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            try
            {
                await _signalRService.PublishRemarkDeletedAsync(@event);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}