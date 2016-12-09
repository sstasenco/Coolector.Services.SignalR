using System.Threading.Tasks;
using Coolector.Services.Remarks.Shared.Events;

namespace Coolector.Services.SignalR.Services
{
    public interface IRemarkSignalRService
    {
        Task PublishRemarkCreatedAsync(RemarkCreated @event);
        Task PublishRemarkResolvedAsync(RemarkResolved @event);
        Task PublishRemarkDeletedAsync(RemarkDeleted @event);
    }
}