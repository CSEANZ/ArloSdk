using System.Collections.Generic;
using System.Threading.Tasks;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Product;

namespace Arlo.SDK.Contract
{
    public interface IContactService
    {
        Task<List<ArloContact>> GetPresenters(ArloSession ev);
        Task<List<ArloContact>> GetContacts(string contactsLink);
        Task<List<ArloContact>> GetContacts(ContactList contacts);
        Task<List<ArloContact>> GetAllEventRegistrationContacts(ArloEvent ev, bool forceRefresh);
        Task<List<ArloRegistration>> GetAllEventRegistrations(ArloEvent ev, bool forceRefresh);
        Task<ArloRegistration> MapContactIdToRegistration(ArloEvent ev, string contactId);
        Task<ArloRegistration> GetRegistration(string registrationId);
    }
}