using System.Collections.Generic;
using System.Threading.Tasks;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Product;

namespace Arlo.SDK.Contract
{
    public interface IArloSessionRegistrationService
    {
        Task<List<ArloEventSessionRegistration>> GetSessionRegistrations(ArloRegistration rego, bool forceRefresh);
        Task<bool> RegisterForSession(ArloRegistration rego, ArloSession session);
        Task<bool> UnregisterFromSession(ArloRegistration rego, ArloEventSessionRegistration session);
        Task<ArloEventSessionRegistration> IsRegistered(ArloRegistration rego, ArloSession session, bool forceRefresh);
        Task<ArloEventSessionRegistration> IsRegistered(ArloRegistration rego, string sessionCode, bool forceRefresh);
    }
}