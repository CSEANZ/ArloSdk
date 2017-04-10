using System;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Services.System;
using Xamling.Azure.Portable.Contract.Cache;


namespace Arlo.SDK.Services
{
    public class EventService : CachingService, IEventService
    {
        private readonly IEventListRepo _eventsRepo;
        private readonly IArloEventRepo _arloEventRepo;

        private static ArloEvent _currentEvent;

        public EventService(IEventListRepo eventsRepo, IArloEventRepo arloEventRepo, IRedisEntityCache redisCache) : base(redisCache)
        {
            _eventsRepo = eventsRepo;
            _arloEventRepo = arloEventRepo;
        }

        public async Task<ArloEvent> GetCurrentEvent()
        {
            return _currentEvent ?? (_currentEvent = await GetEventById("1881", false));
        }

        public async Task<EventList> GetAllEvents()
        {
            var evt = await GetEntity<EventList>(Constants.Cache.AllEvents);

            if (evt != null)
            {
                return evt;
            }

            var result = await _eventsRepo.Get();

            await SetEntity(Constants.Cache.AllEvents, result, Constants.Cache.DefaultTimespan);

            return result;
        }

        public async Task<ArloEvent> GetEventById(string id, bool forceRefresh)
        {
            if (!forceRefresh)
            {
                ArloEvent evt = null;
                evt = await GetEntity<ArloEvent>(_eventKey(id));

                if (evt != null)
                {
                    return evt;
                }
            }

            var evFromServer = await _arloEventRepo.GetById(id);

            await SetEntity(_eventKey(id), evFromServer, Constants.Cache.DefaultTimespan);

            return evFromServer;


        }

        string _eventKey(string id)
        {
            return $"{Constants.Cache.AllEvents}.{id}";
        }


    }
}
