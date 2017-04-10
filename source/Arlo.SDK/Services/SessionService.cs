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
using XamlingCore.Portable.Util.TaskUtils;

namespace Arlo.SDK.Services
{
    public class SessionService : CachingService, ISessionService
    {
        private readonly IContactService _contactService;
        private readonly IEventService _eventService;
        private readonly ICustomFieldsRepo _customFieldsRepo;

        private readonly IEventSessionRepo _eventSessionRepo;
        private readonly IVenueRepo _venueRepo;
        private readonly IRoomRepo _roomRepo;
        private readonly ILogService _logService;
        private readonly ITimeService _timeService;

        public SessionService(IContactService contactService,
            IEventService _eventService,
            ICustomFieldsRepo customFieldsRepo,
            IEventSessionRepo eventSessionRepo,
            IVenueRepo venueRepo,
            IRoomRepo roomRepo,
            ILogService logService,
            ITimeService timeService,
            IRedisEntityCache cache) : base(cache)
        {
            _contactService = contactService;
            this._eventService = _eventService;
            _customFieldsRepo = customFieldsRepo;

            _eventSessionRepo = eventSessionRepo;
            _venueRepo = venueRepo;
            _roomRepo = roomRepo;
            _logService = logService;
            _timeService = timeService;
        }

        /// <summary>
        /// okay - so this is so we can mock it during development
        /// </summary>
        /// <returns></returns>
        public DateTime GetCurrentTime()
        {
            return _timeService.GetQueenslandTime(DateTime.UtcNow);
        }

        public DateTime ConvertTime(DateTime dt)
        {
            return _timeService.GetQueenslandTime(dt);
        }

        public async Task<ArloSession> GetEventSession(string sessionId)
        {
            return await GetEntity<ArloSession>(_getSessionIdKey(sessionId));
        }

        public async Task<ArloSession> GetEventSession(string sessionUrl, bool forceRefresh)
        {
            var result = await GetEntity(_getEventKey(sessionUrl), () => _eventSessionRepo.Get(sessionUrl),
                Constants.Cache.DefaultTimespan, forceRefresh); //await _entityCache.GetEntity(_getEventKey(sessionUrl), () => _eventSessionRepo.Get(sessionUrl));

            if (result == null)
            {
                _logService.TrackTrace("Event Session not found", XSeverityLevel.Critical,
                    new Dictionary<string, string> { { "Url", sessionUrl } });
                return null;
            }

            await Task.WhenAll(
                _contactService.GetPresenters(result),
                _getCustomFields(result, forceRefresh),
                _getVenueDetails(result, false)
            );

            await Task.WhenAll(
                SetEntity(_getEventKey(sessionUrl), result), //set it again with the populated presenters;
                SetEntity(_getSessionIdKey(result), result)//set it so can be retrieved with id
            );

            return result;
        }

        async Task _getVenueDetails(ArloSession session, bool forceRefresh)
        {
            if (session == null)
            {
                return;
            }

            var fieldsLink = session.Link.FirstOrDefault(_ => _.Rel == Constants.Rel.VenueDetails);
            if (fieldsLink == null)
            {
                return;
            }

            var fields = await GetEntity(fieldsLink.Href, () => _venueRepo.Get(fieldsLink.Href), Constants.Cache.DefaultTimespan, forceRefresh);

            session.VenueDetails = fields;

            await _getRoomDetails(session, fields, forceRefresh);
        }

        async Task _getRoomDetails(ArloSession session, EventSessionVenueDetails venueDetails, bool forceRefresh)
        {
            if (session == null || venueDetails == null)
            {
                return;
            }

            var fieldsLink = venueDetails.Link?.FirstOrDefault(_ => _.Rel == Constants.Rel.VenueRoom);
            if (fieldsLink == null)
            {
                return;
            }

            var fields = await GetEntity(fieldsLink.Href, () => _roomRepo.Get(fieldsLink.Href), Constants.Cache.DefaultTimespan, forceRefresh);

            session.VenueDetails.VenueRoom = fields;
        }

        async Task _getCustomFields(ArloSession session, bool forceRefresh)
        {
            if (session == null)
            {
                return;
            }

            var fieldsLink = session.Link.FirstOrDefault(_ => _.Rel == Constants.Rel.CustomFields);
            if (fieldsLink == null)
            {
                return;
            }

            var fields = await GetEntity(fieldsLink.Href, () => _customFieldsRepo.Get(fieldsLink.Href), Constants.Cache.DefaultTimespan, forceRefresh);
            
            session.CustomFields = fields;
        }

        public async Task<List<ArloSession>> GetEventSessionsAsync(ArloEvent ev, bool forceRefresh)
        {
            var sessionList = await GetAllEventSessionLinks(ev, true, false);
            return await GetEventSessions(sessionList.Link, forceRefresh);
        }

        public async Task<List<ArloSession>> GetEventSessionsFilterPresenter(string firstName, string lastName)
        {
            firstName = firstName.ToLower();
            lastName = lastName.ToLower();
            var allSessions = await GetEventSessions(false);

            var filtered = new List<ArloSession>();

            foreach (var i in allSessions)
            {
                var presenters = string.Join(",", i.Presenters.Select(_ => _.FullName)).ToLower();

                if (presenters.Contains(firstName) && presenters.Contains(lastName))
                {
                    filtered.Add(i);
                }
            }

            return filtered;
        }

        public async Task<List<ArloSession>> GetEventSessionsFilterField(string field, string value)
        {
            var allSessions = await GetEventSessions(false);

            var result = allSessions.Where(
                _ => _.CustomFields.Field.Any(
                    _2 => _2.Name == field &&
                          _2.Value.String.ToLower().Replace(" ", "").Contains(value.ToLower().Replace(" ", ""))
                )).ToList();

            return result;
        }

        public async Task<List<string>> GetAllTopics(ArloEvent ev)
        {
            var cache = await GetEntity<List<string>>(Constants.Cache.AllTopics);

            if (cache != null)
            {
                return cache;
            }

            var eventSession = await GetEventSessions(false);

            var overallList = new List<string>();

            foreach (var sess in eventSession)
            {
                if (sess?.CustomFields?.Field == null)
                {
                    continue;
                }

                foreach (var f in sess.CustomFields.Field)
                {
                    if (string.IsNullOrWhiteSpace(f?.Value?.String))
                    {
                        continue;
                    }

                    if (overallList.Contains(f.Value.String))
                    {
                        continue;
                    }
                    if (//f.Name == Constants.Fields.PrimaryProductFocus ||
                        //f.Name == Constants.Fields.Theme ||
                        f.Name == Constants.Fields.Track)
                    {
                        overallList.Add(f.Value.String);
                    }
                }
            }

            await SetEntity(Constants.Cache.AllTopics, overallList);

            return overallList;
        }

        public async Task<List<string>> GetTopicsByDateTime(ArloEvent ev, DateTime date, string time)
        {
            var eventSessions = await GetEventSessions(false);

            var filteredSessions = eventSessions.Where(
                s => FilterSessionByDayAndTime(s, date, time)
            ).ToList();


            var topics = new List<string>();

            foreach (var session in filteredSessions)
            {
                if (session?.CustomFields?.Field == null)
                {
                    continue;
                }

                foreach (var f in session.CustomFields.Field)
                {
                    if (string.IsNullOrWhiteSpace(f?.Value?.String))
                    {
                        continue;
                    }

                    if (topics.Contains(f.Value.String))
                    {
                        continue;
                    }
                    if (f.Name == Constants.Fields.Track)
                    {
                        topics.Add(f.Value.String);
                    }
                }
            }

            return topics;
        }

        private bool FilterSessionByDayAndTime(ArloSession s, DateTime date, string time)
        {
            if (!s.DateTime_Start.HasValue || s.DateTime_Start.Value.Date != date.Date)
            {
                return false;
            }

            if (time.ToLowerInvariant().Equals("morning"))
            {
                if (s.DateTime_Start.Value.TimeOfDay.Hours < 12)
                {
                    return true;
                }
            }
            else
            {
                if (s.DateTime_Start.Value.TimeOfDay.Hours >= 12)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<string>> GetAllPresenters(ArloEvent ev)
        {
            var cache = await GetEntity<List<string>>(Constants.Cache.AllPresenters);

            if (cache != null)
            {
                return cache;
            }

            var eventSession = await GetEventSessions(false);

            var allPresenters = new List<string>();

            foreach (var session in eventSession)
            {
                foreach (var presenter in session.Presenters)
                {
                    var fullName = presenter.FullName;


                    if (!allPresenters.Contains(fullName))
                    {
                        allPresenters.Add(fullName);
                    }
                }
            }

            await SetEntity(Constants.Cache.AllPresenters, allPresenters);
            return allPresenters;
        }

        public async Task<List<ArloSession>> GetEventSessions(bool forceRefresh)
        {
            if (!forceRefresh)
            {
                var cache = await GetEntity<List<ArloSession>>(Constants.Cache.AllSessionsPopulated);

                if (cache != null)
                {
                    return cache;
                }
            }

            var links = await GetAllEventSessionLinks(await _eventService.GetCurrentEvent(), true, forceRefresh);

            var listResult = await GetEventSessions(links.Link, forceRefresh);

            var resultFiltered = listResult.Where(
               _ => _?.CustomFields?.Field != null && _.CustomFields.Field.Any(
                        _2 => _2?.Name == Constants.Fields.ContentReviewStatus &&
                              _2?.Value?.String == Constants.Fields.Values.Confirmed)
               ).ToList();

            //order them by time

            var ordered = resultFiltered.Where(_ => _?.DateTime_Start != null).OrderBy(_ => _.DateTime_Start).ToList();

            await SetEntity(Constants.Cache.AllSessionsPopulated, ordered);

            if (forceRefresh)
            {
                _logService.TrackMetric("SessionCount", ordered.Count);
            }

            return ordered;
        }

        public async Task<List<ArloSession>> GetEventSessions(List<Link> sessionLinks, bool forceRefresh)
        {
            if (sessionLinks == null)
            {
                return new List<ArloSession>();
            }

            var resultTasks = new List<Task<ArloSession>>();

            foreach (var item in sessionLinks.Where(_ => _.Rel == Constants.Rel.EventSession))
            {
                var i2 = item;
                var t = TaskThrottler.Get("GetSessonLinks", 30).Throttle(() => GetEventSession(i2.Href, forceRefresh));
                resultTasks.Add(t);
            }

            var resultFromTasks = await Task.WhenAll(resultTasks);

            var listResult = resultFromTasks.ToList();

            return listResult;
        }



        public async Task<SessionList> GetAllEventSessionLinks(ArloEvent ev, bool doNext, bool forceRefresh)
        {

            var sessionList = await RecurseGetLinks<SessionList>(ev, _getEventSessionsKey(ev), Constants.Rel.Sessions,
                true, forceRefresh);

            return sessionList;
        }

        string _getEventSessionsKey(ArloEvent ev)
        {
            return $"sessions.{ev.EventID}";
        }

        string _getEventKey(string eventUrl)
        {
            return $"event.{eventUrl}";
        }

        string _getSessionIdKey(ArloSession session)
        {
            return $"session.{session.SessionID}";
        }

        string _getSessionIdKey(string sessionId)
        {
            return $"session.{sessionId}";
        }
    }
}
