using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Coolector.Common.Events.Remarks;
using Coolector.Common.Events.Remarks.Models;
using Coolector.Services.SignalR.Hubs;
using Coolector.Services.SignalR.Services;
using Machine.Specifications;
using Microsoft.AspNetCore.SignalR;
using Moq;
using It = Machine.Specifications.It;

namespace Coolector.Services.SignalR.Tests.Specs.Services
{
    public abstract class RemarkSignalRService_specs
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

        protected static T Cast<T>(T example, object o)
        {
            IComparer<string> comparer = StringComparer.CurrentCultureIgnoreCase;
            //Get constructor with lowest number of parameters and its parameters 
            var constructor = typeof(T).GetConstructors(
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            ).OrderBy(c => c.GetParameters().Length).First();
            var parameters = constructor.GetParameters();

            //Get properties of input object
            var sourceProperties = new List<PropertyInfo>(o.GetType().GetProperties());
            if (parameters.Length <= 0)
                return (T) constructor.Invoke(null);

            var values = new object[parameters.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Type t = parameters[i].ParameterType;
                //See if the current parameter is found as a property in the input object
                var source = sourceProperties.Find(item => comparer.Compare(item.Name, parameters[i].Name) == 0);

                //See if the property is found, is readable, and is not indexed
                if (source != null && source.CanRead &&
                    source.GetIndexParameters().Length == 0)
                {
                    //See if the types match.
                    if (source.PropertyType == t)
                    {
                        //Get the value from the property in the input object and save it for use
                        //in the constructor call.
                        values[i] = source.GetValue(o, null);
                        continue;
                    }
                    //See if the property value from the input object can be converted to
                    //the parameter type
                    try
                    {
                        values[i] = Convert.ChangeType(source.GetValue(o, null), t);
                        continue;
                    }
                    catch
                    {
                        //Impossible. Forget it then.
                    }
                }
                //If something went wrong (i.e. property not found, or property isn't 
                //converted/copied), get a default value.
                values[i] = t.GetTypeInfo().IsValueType ? Activator.CreateInstance(t) : null;
            }
            //Call the constructor with the collected values and return it.
            return (T)constructor.Invoke(values);
            //Call the constructor without parameters and return the it.
        }
    }

    [Subject("RemarkSignalRService PublishRemarkCreatedAsync")]
    public class When_publish_remark_created : RemarkSignalRService_specs
    {
        Establish context = () => Initialize();

        Because of = () => SignalRService.PublishRemarkCreatedAsync(RemarkCreated).Await();

        It should_invoke_async_method_on_clients = () =>
        {
            ClientProxyMock.Verify(x => x.InvokeAsync("RemarkCreated", Moq.It.Is<object[]>(objects => VerifyArgs(objects))), Times.Once);
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
}