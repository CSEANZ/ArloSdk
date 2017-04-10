using System;

namespace Arlo.SDK.Entities.System
{
    public static class Constants
    {
        public const string ArloEventId = "1881";

        public const string ArloUsername = "ArloUsername";
        public const string ArloPassword = "ArloPassword";

        public const string XmlType = "application/xml";

        public static class Search
        {
            public const string SearchServiceName = "SearchServiceName";
            public const string SearchServiceAdminApiKey = "SearchServiceAdminApiKey";
            public const string SearchIndexName = "ignite";
        }

        public static class Fields
        {
            public const string PrimaryProductFocus = "PrimaryProductFocus";
            public const string Theme = "Theme";
            public const string Track = "Track";
            public const string ContentReviewStatus = "ContentReviewStatus";
            public const string Objective = "Objective";
            public const string Audience = "Audience";
            public const string SessionCode = "SessionCode";

            public static class Values
            {
                public const string Confirmed = "Confirmed";
            }
        }

        public static class Rel
        {
            public const string Self = "self";

            public const string VenueRoom = "http://schemas.learningsourceapp.com/api/2012/02/auth/related/VenueRoom";
            public const string VenueDetails =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/VenueDetails";
            public const string Sessions = "http://schemas.learningsourceapp.com/api/2012/02/auth/related/Sessions";
            public const string EventSession = "http://schemas.learningsourceapp.com/api/2012/02/auth/EventSession";
            public const string Presenters = "http://schemas.learningsourceapp.com/api/2012/02/auth/related/Presenters";
            public const string Contact = "http://schemas.learningsourceapp.com/api/2012/02/auth/Contact";
            public const string RelatedContact = "http://schemas.learningsourceapp.com/api/2012/02/auth/related/Contact";
            public const string CustomFields =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/CustomFields";

            public const string Registrations =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/Registrations";

            public const string Registration =
               "http://schemas.learningsourceapp.com/api/2012/02/auth/Registration";

            public const string SessionRegistrations =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/SessionRegistrations";

            public const string RelatedSession = 
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/Session";


            public const string EventSessionRegistration =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/EventSessionRegistration";

            public const string ParentRegistration =
                "http://schemas.learningsourceapp.com/api/2012/02/auth/related/ParentRegistration";

            public const string Next = "next";

        }

        public static class Endpoints
        {
            public const string Events = "resources/events/";


            public const string EventSessionPattern =
                "resources/eventsessions/{0}/";

            public const string ParentRegistrationPattern =
                "resources/registrations/{0}/";

            public const string SessionRegistrationsPattern =
                "resources/registrations/{0}/sessionregistrations/";

            //#sigh 2 for 22
            public const string SessionRegistrationsPattern2 =
                "sessionregistrations/{0}";
        }

        public static class Cache
        {
            public const string AllEvents = "AllEvents";
            public const string AllSessionsPopulated = "AllSessionsPopulated";
            public const string AllContacts = "AllContacts";
            public const string AllRegistrations = "AllRegistrations";
            public const string AllRegistrationList = "AllRegistrationList";
            public const string AllRegistrationContacts = "AllRegistrationContacts";
            public const string AllRegistrationsComplete = "AllRegistrationsComplete";
            public const string AllRegistrationContactsComplete = "AllRegistrationContactsComplete";
            public const string Contact = "Contact.{0}";
            public const string Registration = "Registration.{0}";
            public const string AllTopics = "AllTopics";
            public const string AllPresenters = "AllPresenters";
            public static TimeSpan DefaultTimespan = TimeSpan.FromHours(6);
            public static TimeSpan MediumTimespan = TimeSpan.FromHours(3);
            public static TimeSpan ShortTimespan = TimeSpan.FromMinutes(5);
        }

       
    }
}
