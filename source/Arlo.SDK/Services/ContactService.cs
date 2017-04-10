using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Services.System;
using Arlo.SDK.Util;
using Xamling.Azure.Portable.Contract;
using Xamling.Azure.Portable.Contract.Cache;
using XamlingCore.Portable.Model.Other;
using XamlingCore.Portable.Util.TaskUtils;

namespace Arlo.SDK.Services
{
    public class ContactService : CachingService, IContactService
    {
        private readonly IContactListRepo _contactListRepo;
        private readonly IContactRepo _contactRepo;
        private readonly IArloRegistrationSearchRepo _registrationSearch;
        private readonly ILogService _logService;

        public ContactService(IContactListRepo contactListRepo, 
            IContactRepo contactRepo, 
            IArloRegistrationSearchRepo registrationSearch, 
            ILogService logService,
            IRedisEntityCache entityCache) : base(entityCache)
        {
            _contactListRepo = contactListRepo;
            _contactRepo = contactRepo;
            _registrationSearch = registrationSearch;
            _logService = logService;
        }

        public async Task<ArloRegistration> GetRegistration(string registrationId)
        {
            var result = await LoadEntity<ArloRegistration>(
                string.Format(Constants.Cache.Registration, registrationId),
                string.Format(Constants.Endpoints.ParentRegistrationPattern, registrationId));

            if (result == null)
            {
                return null;
            }

            if (result.Contact != null)
            {
                return result;
            }

            var link = result.FindLink(Constants.Rel.RelatedContact);

            result.Contact = await LoadEntity<ArloContact>(link, link);

            await SetEntity(string.Format(Constants.Cache.Registration, registrationId), result); //set it with the contact. 

            return result;
        }

        public async Task<ArloRegistration> MapContactIdToRegistration(ArloEvent ev, string contactId)
        {
            var cache = await GetEntity<ArloRegistration>(string.Format(Constants.Cache.Contact, contactId));

            if (cache != null)
            {
                return cache;
            }

            //this guy is not present so need to resolve it using search
            var searchRego = await _registrationSearch.Search(ev, contactId);

            if (searchRego == null)
            {
                return null;
            }

            var registrants = await LoadLinkEntities<ArloRegistration>(searchRego, Constants.Cache.AllRegistrationList + $".{contactId}", Constants.Rel.Registration, true);

            var registrant = registrants.FirstOrDefault();

            var link = registrant.FindLink(Constants.Rel.RelatedContact);

            if (link == null)
            {
                return null;
            }

            registrant.Contact = await LoadEntity<ArloContact>(link, link);

            await SetEntity(string.Format(Constants.Cache.Contact, contactId), registrant);

            return registrant;
        }

        public async Task<List<ArloRegistration>> GetAllEventRegistrations(ArloEvent ev, bool forceRefresh)
        {
            var cache = await GetEntity<List<ArloRegistration>>(Constants.Cache.AllRegistrationsComplete);

            if (cache != null && !forceRefresh)
            {
                return cache;
            }

            var links = await RecurseGetLinks<EventRegistrationList>(ev, Constants.Cache.AllRegistrations, Constants.Rel.Registrations,
                true, forceRefresh);

            var registrants = await _getRegistrationLinks(links, forceRefresh);

            var taskListUpdateCache = new List<Task>();

            foreach (var r in registrants)
            {
                if (r.Contact == null)
                {
                    continue;
                }
                taskListUpdateCache.Add(SetEntity(string.Format(Constants.Cache.Contact, r.Contact.ContactID), r));
                taskListUpdateCache.Add(SetEntity(string.Format(Constants.Cache.Registration, r.RegistrationID), r));
            }

            await Task.WhenAll(taskListUpdateCache);

            await SetEntity(Constants.Cache.AllRegistrationsComplete, registrants);

            return registrants;
            
        }

        async Task<List<ArloRegistration>> _getRegistrationLinks(EventRegistrationList links, bool forceRefresh)
        {
            var registrants = await LoadLinkEntities<ArloRegistration>(links, Constants.Cache.AllRegistrationList, Constants.Rel.Registration, forceRefresh);

            var taskList = new List<Task>();

            foreach (var registrant in registrants)
            {
                var r2 = registrant;

                async Task<bool> LocalLoad()
                {
                    var link = r2.FindLink(Constants.Rel.RelatedContact);
                    if (link == null)
                    {
                        return false;
                    }

                    var c = await LoadEntity<ArloContact>(link, link);

                    r2.Contact = c;
                    return true;
                }

                taskList.Add(TaskThrottler.Get("LoadRegistrants", 15).Throttle(LocalLoad));
            }

            await Task.WhenAll(taskList);

            return registrants;
        }

        public async Task<List<ArloContact>> GetAllEventRegistrationContacts(ArloEvent ev, bool forceRefresh)
        {
            var cache = await GetEntity<List<ArloContact>>(Constants.Cache.AllRegistrationContactsComplete);

            if (cache != null && !forceRefresh)
            {
                return cache;
            }

            var links = await RecurseGetLinks<EventRegistrationList>(ev, Constants.Cache.AllRegistrations, Constants.Rel.Registrations,
                true, forceRefresh);

            var registrants = await LoadLinkEntities<ArloRegistration>(links, Constants.Cache.AllRegistrationList, Constants.Rel.Registration, forceRefresh);
            

            var contacts = await LoadLinkEntitiesFromList<ArloContact, ArloRegistration>(registrants, Constants.Cache.AllRegistrationContacts,
                Constants.Rel.RelatedContact, forceRefresh);

            if (contacts == null)
            {
                _logService.TrackTrace("NullContacts!", XSeverityLevel.Critical);
                return null;
            }

            await SetEntity(Constants.Cache.AllRegistrationContactsComplete, contacts);

            if (forceRefresh)
            {
                _logService.TrackMetric("EventRegistrations", contacts.Count);
            }

            return contacts;
        }

        public async Task<List<ArloContact>> GetPresenters(ArloSession ev)
        {
            var link = ev?.Link.FirstOrDefault(_ => _.Rel == Constants.Rel.Presenters);

            if (link == null)
            {
                return null;
            }

            var contacts = await GetContacts(link.Href);

            ev.Presenters = contacts;
            return contacts;
        }

        public async Task<List<ArloContact>> GetContacts(string contactsLink)
        {
            var contactList = await _getContactList(contactsLink);

            if (contactList == null)
            {
                return null;
            }

            return await GetContacts(contactList);
        }

        public async Task<List<ArloContact>> GetContacts(ContactList contacts)
        {
            if (contacts == null)
            {
                return null;
            }

            var list = new List<ArloContact>();

            foreach (var contactLink in contacts.Link.Where(_=>_.Rel == Constants.Rel.Contact))
            {
                var contact = await _getContact(contactLink.Href);
                list.Add(contact);
            }

            return list;
        }

        async Task<ArloContact> _getContact(string contactLink)
        {
            return await GetEntity(contactLink, () => _contactRepo.Get(contactLink));
        }

        async Task<ContactList> _getContactList(string contactsLink)
        {
            return await GetEntity(contactsLink, ()=> _contactListRepo.Get(contactsLink));
        }
    }
}
