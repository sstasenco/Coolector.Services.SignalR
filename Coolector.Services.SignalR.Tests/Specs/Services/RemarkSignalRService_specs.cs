using System;
using System.Collections.Generic;
using System.Linq;
using Coolector.Services.Remarks.Shared.Events;
using Coolector.Services.Remarks.Shared.Events.Models;
using Coolector.Services.SignalR.Hubs;
using Coolector.Services.SignalR.Services;
using Coolector.Services.SignalR.Tests.Framework;
using Machine.Specifications;
using Microsoft.AspNetCore.SignalR;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Services.SignalR.Tests.Specs.Services
{
    public abstract class RemarkSignalRService_specs : SignalRServicesSpecsBase
    {
        protected static IRemarkSignalRService SignalRService;
        protected static Mock<IHubContext<RemarksHub>> HubContextMock;
        protected static Mock<IHubConnectionContext<IClientProxy>> HubConnectionProxyMock;
        protected static Mock<IClientProxy> ClientProxyMock;

        protected static string UserId = "userId";
        protected static string Username = "username";
        protected static Guid RequestId = Guid.NewGuid();
        protected static Guid RemarkId = Guid.NewGuid();
        protected static Guid CategoryId = Guid.NewGuid();
        protected static string Category = "litter";
        protected static string Description = "Description";
        protected static DateTime CreatedAt = DateTime.Now;
        protected static DateTime ResolvedAt = DateTime.Now;
        protected static string SmallPhotoUrl = "url";

        protected static RemarkCreated RemarkCreated;
        protected static RemarkDeleted RemarkDeleted;
        protected static RemarkResolved RemarkResolved;

        protected static void Initialize()
        {
            var smallPhoto = new RemarkFile("name", "small", SmallPhotoUrl, null);

            RemarkCreated = new RemarkCreated(RequestId, RemarkId, UserId, Username,
                new RemarkCreated.RemarkCategory(CategoryId, Category),
                new RemarkCreated.RemarkLocation(string.Empty, 1, 1), new List<RemarkFile> { smallPhoto },
                Description, CreatedAt);
            RemarkDeleted = new RemarkDeleted(RequestId, RemarkId, UserId);
            RemarkResolved = new RemarkResolved(RequestId, RemarkId, UserId, Username,
                new List<RemarkFile>(), ResolvedAt);

            HubContextMock = new Mock<IHubContext<RemarksHub>>();
            HubConnectionProxyMock = new Mock<IHubConnectionContext<IClientProxy>>();
            ClientProxyMock = new Mock<IClientProxy>();
            SignalRService = new RemarkSignalRService(HubContextMock.Object);

            HubContextMock.Setup(x => x.Clients).Returns(HubConnectionProxyMock.Object);
            HubConnectionProxyMock.Setup(x => x.All).Returns(ClientProxyMock.Object);
        }
    }

    [Subject("RemarkSignalRService PublishRemarkCreatedAsync")]
    public class When_publish_remark_created : RemarkSignalRService_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignalRService.PublishRemarkCreatedAsync(RemarkCreated).Await();

        It should_invoke_async_method_on_clients = () =>
        {
            ClientProxyMock.Verify(x => x.InvokeAsync("RemarkCreated", 
                Moq.It.Is<object[]>(objects => VerifyArgs(objects))), Times.Once);
        };

        protected static bool VerifyArgs(object[] args)
        {
            var a = new
            {
                id = new Guid(), author = "", category = "",
                location = new { address = "", coordinates = new[] {0.0, 0.0}, type = ""},
                smallPhotoUrl = "", description = "",
                createdAt = default(DateTime), resolved = default(bool)
            };
            var message = Cast(a, args.First());
            return message.id == RemarkId
                   && message.author == Username
                   && message.category == Category
                   && message.smallPhotoUrl == SmallPhotoUrl
                   && message.description == Description
                   && message.createdAt == CreatedAt
                   && message.resolved == false;
        }
    }

    [Subject("RemarkSignalRService PublishRemarkResolvedAsync")]
    public class When_publish_remark_resolved : RemarkSignalRService_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignalRService.PublishRemarkResolvedAsync(RemarkResolved).Await();

        It should_invoke_async_method_on_clients = () =>
        {
            ClientProxyMock.Verify(x => x.InvokeAsync("RemarkResolved", 
                Moq.It.Is<object[]>(o => VerifyArgs(o))), Times.Once);
        };

        protected static bool VerifyArgs(object[] args)
        {
            var a = new
            {
                remarkId = Guid.Empty, resolverId = "",
                resolver = "", resolvedAt = default(DateTime)
            };
            var message = Cast(a, args.First());
            return message.remarkId == RemarkId
                   && message.resolverId == UserId
                   && message.resolver == Username
                   && message.resolvedAt == ResolvedAt;
        }
    }

    [Subject("RemarkSignalRService PublishRemarkDeletedAsync")]
    public class When_publish_remark_deleted : RemarkSignalRService_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignalRService.PublishRemarkDeletedAsync(RemarkDeleted).Await();

        It should_invoke_async_method_on_clients = () =>
        {
            ClientProxyMock.Verify(x => x.InvokeAsync("RemarkDeleted",
                Moq.It.Is<object[]>(o => VerifyArgs(o))), Times.Once);
        };

        protected static bool VerifyArgs(object[] args)
        {
            var a = new { remarkId = Guid.Empty };
            var message = Cast(a, args.First());
            return message.remarkId == RemarkId;
        }
    }
}