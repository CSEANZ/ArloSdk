using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Base;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Services.System;
using Arlo.SDK.Util;
using Xamling.Azure.Portable.Contract;
using Xamling.Azure.Portable.Contract.Cache;
using XamlingCore.Portable.Model.Other;

namespace Arlo.SDK.Services
{
    public class ArloSessionRegistrationService : CachingService, IArloSessionRegistrationService
    {
        private readonly ISessionService _sessionService;
        private readonly ISystemService _systemService;
        private readonly IGenericWebRepo<ArloEventSessionRegistration> _regoRepo;
        private readonly ILogService _logService;

        public ArloSessionRegistrationService(ISessionService sessionService, 
            ISystemService systemService, IGenericWebRepo<ArloEventSessionRegistration> regoRepo, ILogService logService, IRedisEntityCache entityCache) : base(entityCache)
        {
            _sessionService = sessionService;
            _systemService = systemService;
            _regoRepo = regoRepo;
            _logService = logService;
        }

        public async Task<ArloEventSessionRegistration> IsRegistered(ArloRegistration rego, ArloSession session, bool forceRefresh)
        {
            var registrations = await GetSessionRegistrations(rego, forceRefresh);

            var thisRego = registrations.FirstOrDefault(_ => _.Session?.SessionID == session.SessionID);

            return thisRego;
        }

        public async Task<ArloEventSessionRegistration> IsRegistered(ArloRegistration rego, string sessionCode, bool forceRefresh)
        {
            var registrations = await GetSessionRegistrations(rego, forceRefresh);

            var thisRego = registrations.FirstOrDefault(_ => _.Session?.Code == sessionCode);

            return thisRego;
        }

        public async Task<bool> UnregisterFromSession(ArloRegistration rego, ArloEventSessionRegistration session)
        {
            var result = await _regoRepo.UploadRaw(null,
                $"{rego.FindLink(Constants.Rel.Self)}{string.Format(Constants.Endpoints.SessionRegistrationsPattern2, session.RegistrationID)}",
                "DELETE");

            if (result.IsSuccessCode)
            {
                await GetSessionRegistrations(rego, true); //recache this
                _logService.TrackTrace("Unregsiter", XSeverityLevel.Error,

                    new Dictionary<string, string>
                    {
                        {
                            "text", result.Result
                        },
                        {"rego", rego.RegistrationID },
                        {"registrationactualId", session.RegistrationID }
                    });
            }
            else
            {
                _logService.TrackTrace("Unregister issue", XSeverityLevel.Error, 
                    
                    new Dictionary<string, string>
                    {
                        {
                            "text", result.Result
                        },
                        {"rego", rego.RegistrationID },
                        {"registrationactualId", session.RegistrationID }
                    });
            }

            return result.IsSuccessCode;
        }

        public async Task<bool> RegisterForSession(ArloRegistration rego, ArloSession session)
        {
            var evRego = new ArloEventSessionRegistration
            {
                Link = new List<Link>()
            };

            var linkSession = new Link
            {
                Rel = Constants.Rel.RelatedSession,
                Type = Constants.XmlType,
                Title = "Session",
                Href = _systemService.GetRealUrl(string.Format(Constants.Endpoints.EventSessionPattern, session.SessionID))
            };

            var linkRego = new Link
            {
                Rel = Constants.Rel.ParentRegistration,
                Type = Constants.XmlType,
                Title = "ParentRegistration",
                Href = _systemService.GetRealUrl(string.Format(Constants.Endpoints.ParentRegistrationPattern, rego.RegistrationID))
            };

            evRego.Link.Add(linkRego);
            evRego.Link.Add(linkSession);

            var result = await _regoRepo.PostResult(evRego,
                string.Format(Constants.Endpoints.SessionRegistrationsPattern, rego.RegistrationID));

            if (result.IsSuccessCode)
            {
                await GetSessionRegistrations(rego, true); //recache this
                _logService.TrackTrace("Register", XSeverityLevel.Information,

                  new Dictionary<string, string>
                  {
                        {
                            "text", result.Result
                        },
                        {"rego", rego.RegistrationID },
                        {"sessionid", session.SessionID }
                  });
            }
            else
            {
                _logService.TrackTrace("Unregister issue", XSeverityLevel.Error,

                    new Dictionary<string, string>
                    {
                        {
                            "text", result.Result
                        },
                        {"rego", rego.RegistrationID },
                        {"sessionid", session.SessionID }
                    });
            }

            return result.IsSuccessCode;
        }

        public async Task<List<ArloEventSessionRegistration>>  GetSessionRegistrations(ArloRegistration rego, bool forceRefresh)
        {
            if (!forceRefresh)
            {
                var cache = await GetEntity<List<ArloEventSessionRegistration>>(rego.RegistrationID);
                if (cache != null)
                {
                    return cache;
                }
            }

            var l = rego.FindLink(Constants.Rel.SessionRegistrations);

            var list = await LoadEntity<EventSessionRegistrationList>(l, l, forceRefresh, Constants.Cache.ShortTimespan);

            var expandedList = await LoadLinkEntities<ArloEventSessionRegistration>(list, l,
                Constants.Rel.EventSessionRegistration, false);

            //Tryng out something with tasks and Local Functions
            async Task SessionDetailGetter(ArloEventSessionRegistration sessionRego)
            {
                var session =
                    await _sessionService.GetEventSession(sessionRego.FindLink(Constants.Rel.RelatedSession), forceRefresh);
                sessionRego.Session = session;
            }

            await expandedList.WhenAllList(SessionDetailGetter);

            expandedList = expandedList.OrderBy(_ => _.Session.DateTime_Start).ToList();

            await SetEntity(rego.RegistrationID, expandedList);

            return expandedList;

        }
    }
}
