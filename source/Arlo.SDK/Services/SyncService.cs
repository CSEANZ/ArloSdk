using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Model.Other;

namespace Arlo.SDK.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILogService _logService;
        private readonly IEventService _eventService;
        private readonly ISessionService _sessionService;
        private readonly IContactService _contactService;

        public SyncService(ILogService logService, 
            IEventService eventService,
            ISessionService sessionService,
            IContactService contactService
            
             )
        {
            _logService = logService;
            _eventService = eventService;
            _sessionService = sessionService;
            _contactService = contactService;
        }
        public async Task<bool> SyncAll()
        {
            _trackTrace("Begin sync all");

            _trackTrace("Begin sync events");

            bool evtGood;
            bool sessionsGood;
            bool contactsGood;

            var evt = await _eventService.GetEventById("1881", true);
            evtGood = evt != null;
            _trackTrace("End sync events");

            _trackTrace("Begin sync sessions");
            var sessions = await _sessionService.GetEventSessions(true);
            sessionsGood = sessions != null && sessions.Count > 0;
            _trackTrace("End sync sessions");


            _trackTrace("Begin sync contacts");
            var contacts = await _contactService.GetAllEventRegistrationContacts(evt, true);

            contactsGood = contacts != null && contacts.Count > 0;

            _trackTrace("End sync contacts");


            var result = evtGood && sessionsGood && contactsGood;

            _logService.TrackEvent($"Sync Complete {result}", new Dictionary<string, string>
            {
                {"Event", evtGood.ToString() },
                {"Sessions", sessionsGood.ToString() },
                {"Contacts", contactsGood.ToString() },
                {"SessionsCount", sessions?.Count.ToString() },
                {"ContactsCount", contacts?.Count.ToString() }
            });
            

            return evtGood && sessionsGood && contactsGood;
        }

        void _trackTrace(string trace)
        {
            _logService.TrackTrace(trace);
            Debug.WriteLine(trace);
        }
    }
}
