using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Base;
using Arlo.SDK.Entities.Product;

namespace Arlo.SDK.Contract
{
    public interface ISessionService
    {
        Task<SessionList> GetAllEventSessionLinks(ArloEvent ev, bool doNext, bool forceRefresh);
        Task<ArloSession> GetEventSession(string eventUrl, bool forceRefresh);
        Task<List<ArloSession>> GetEventSessions(List<Link> sessionLinks, bool forceRefresh);
        Task<List<ArloSession>> GetEventSessionsAsync(ArloEvent ev, bool forceRefresh);
        Task<List<ArloSession>> GetEventSessions(bool forceRefresh);
        Task<List<string>> GetAllTopics(ArloEvent ev);
        Task<List<string>> GetTopicsByDateTime(ArloEvent ev, DateTime date, string time);
        Task<List<string>> GetAllPresenters(ArloEvent ev);
        Task<List<ArloSession>> GetEventSessionsFilterField(string field, string value);

        /// <summary>
        /// okay - so this is so we can mock it during development
        /// </summary>
        /// <returns></returns>
        DateTime GetCurrentTime();

        Task<List<ArloSession>> GetEventSessionsFilterPresenter(string firstName, string lastName);
        DateTime ConvertTime(DateTime dt);
        Task<ArloSession> GetEventSession(string sessionId);
    }
}