﻿using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Coolector.Services.SignalR.Handlers
{
    public class RemarkCreatedHandler : IEventHandler<RemarkCreated>
    {
        private readonly IHubContext<RemarksHub> _hubContext;

        public RemarkCreatedHandler(IHubContext<RemarksHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task HandleAsync(RemarkCreated @event)
        {
            await _hubContext.Clients.All.InvokeAsync("RemarkCreated", @event);
        }
    }
}