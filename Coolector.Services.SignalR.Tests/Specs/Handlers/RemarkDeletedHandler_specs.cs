using System;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.SignalR.Handlers;
using Coolector.Services.SignalR.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Services.SignalR.Tests.Specs.Handlers
{
    public abstract class RemarkDeletedHandler_specs
    {
        protected static Mock<IRemarkSignalRService> SignalRServiceMock;
        protected static RemarkDeletedHandler Handler;
        protected static RemarkDeleted Event;

        protected static string UserId = "userId";
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Guid RequestId = Guid.NewGuid();
        
        protected static void Initialize()
        {
            SignalRServiceMock = new Mock<IRemarkSignalRService>();
            Event = new RemarkDeleted(RequestId, RemarkId, UserId);
            Handler = new RemarkDeletedHandler(SignalRServiceMock.Object);
        }
    }

    [Subject("RemarkDeletedHandler HandleAsync")]
    public class When_handling_remark_deleted_event : RemarkDeletedHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_publish_method_on_signalr_service = () =>
        {
            SignalRServiceMock.Verify(x => x.PublishRemarkDeletedAsync(Moq.It.Is<RemarkDeleted>
                (rc => rc.RequestId == RequestId
                && rc.Id == RemarkId
                && rc.UserId == UserId)), Times.Once);
        };
    }
}