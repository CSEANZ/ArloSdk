using System.Threading.Tasks;
using Arlo.SDK.Entities.Product;
using XamlingCore.Portable.Model.Response;

namespace Arlo.SDK.Contract
{
    public interface IEventService
    {
        Task<EventList> GetAllEvents();
        Task<ArloEvent> GetEventById(string id, bool forceRefresh);
        Task<ArloEvent> GetCurrentEvent();
    }
}