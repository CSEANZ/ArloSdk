using System.Threading.Tasks;

namespace Arlo.SDK.Contract
{
    public interface ISyncService
    {
        Task<bool> SyncAll();
    }
}