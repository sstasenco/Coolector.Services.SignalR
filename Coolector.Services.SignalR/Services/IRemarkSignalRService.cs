using System.Threading.Tasks;
using Coolector.Common.Events.Remarks;

namespace Coolector.Services.SignalR.Services
{
    public interface IRemarkSignalRService
    {
        Task PublishRemarkCreatedAsync(RemarkCreated @event);
        Task PublishRemarkResolvedAsync(RemarkResolved @event);
        Task PublishRemarkDeletedAsync(RemarkDeleted @event);
    }
}