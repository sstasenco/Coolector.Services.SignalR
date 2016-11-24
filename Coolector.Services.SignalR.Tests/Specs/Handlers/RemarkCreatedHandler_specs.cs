using System;
using System.Collections.Generic;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Coolector.Services.SignalR.Handlers;
using Coolector.Services.SignalR.Services;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Services.Storage.Tests.Specs.Handlers
{
    public abstract class RemarkCreatedHandler_specs
    {
        protected static Mock<IRemarkSignalRService> SignalRServiceMock;
        protected static RemarkCreatedHandler Handler;
        protected static RemarkCreated Event;
        protected static Exception Exception;

        protected static string UserId = "userId";
        protected static string Username = "username";
        protected static Guid RequestId = Guid.NewGuid();
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Guid CategoryId = Guid.NewGuid();
        protected static string Category = "litter";

        protected static void Initialize()
        {
            SignalRServiceMock = new Mock<IRemarkSignalRService>();

            Event = new RemarkCreated(RequestId, RemarkId, UserId, Username,
                new RemarkCreated.RemarkCategory(CategoryId, Category),
                new RemarkCreated.RemarkLocation(string.Empty, 1, 1), new List<RemarkFile>(),
                "test", DateTime.Now);

            Handler = new RemarkCreatedHandler(SignalRServiceMock.Object);
        }
    }

    [Subject("RemarkCreatedHandler HandleAsync")]
    public class When_handling_remark_created_event : RemarkCreatedHandler_specs
    {
        Establish context = () => Initialize();

        Because of = () => Handler.HandleAsync(Event).Await();

        It should_call_publish_method_on_signalr_service = () =>
        {
            SignalRServiceMock.Verify(x => x.PublishRemarkCreatedAsync(Moq.It.Is<RemarkCreated>
                (rc => rc.RequestId == RequestId
                && rc.RemarkId == RemarkId
                && rc.UserId == UserId
                && rc.Username == Username
                && rc.Category.CategoryId == CategoryId
                && rc.Category.Name == Category)));
        };
    }
}