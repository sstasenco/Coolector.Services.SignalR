using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Coolector.Services.SignalR.Handlers;
using Coolector.Services.SignalR.Services;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public class RemarkResolvedHandler_specs
    {
        protected static Mock<IRemarkSignalRService> SignalRServiceMock;
        protected static RemarkResolvedHandler Handler;
        protected static RemarkResolved Event;

        protected static string UserId = "userId";
        protected static string Username = "username";
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Guid RequestId = Guid.NewGuid();
        protected static DateTime ResolvedAt = DateTime.Now;

        protected static void Initialize()
        {
            SignalRServiceMock = new Mock<IRemarkSignalRService>();
            Event = new RemarkResolved(RequestId, RemarkId, UserId, Username, 
                new List<RemarkFile>(), ResolvedAt);
            Handler = new RemarkResolvedHandler(SignalRServiceMock.Object);
        }
    }

    [Subject("RemarkResolvedHandler HandleAsync")]
    public class When_handling_remark_resolved_event : RemarkResolvedHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_publish_method_on_signalr_service = () =>
        {
            SignalRServiceMock.Verify(x => x.PublishRemarkResolvedAsync(Moq.It.Is<RemarkResolved>
                (@event => @event.RequestId == RequestId
                && @event.RemarkId == RemarkId
                && @event.UserId == UserId
                && @event.Username== Username
                && @event.ResolvedAt == ResolvedAt)));
        };
    }
}