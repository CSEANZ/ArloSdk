using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities.Base;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Util;
using Autofac;
using Xamling.Azure.Portable.Contract.Cache;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Helpers;
using XamlingCore.Portable.Util.TaskUtils;

namespace Arlo.SDK.Services.System
{
    public class CachingService : ICachingService
    {
        private readonly IRedisEntityCache _entityCache;
       

        public ILifetimeScope Scope { get; set; }
        public IHashHelper HashHelper { get; set; }

        public CachingService(IRedisEntityCache entityCache)
        {
            _entityCache = entityCache;
        }

        public async Task SetEntity<T>(string key, T entity, TimeSpan? ts = null)
            where T:class, new()
        {
            await _entityCache.SetEntity(key, entity, ts ?? Constants.Cache.DefaultTimespan);
        }

        public async Task<T> GetEntity<T>(string key)
             where T : class, new()
        {
            return await _entityCache.GetEntity<T>(key);
        }

        public async Task<T> GetEntity<T>(string key, Func<Task<T>> sourceTask, TimeSpan? ts = null, bool forceRefresh = false)
            where T : class, new()
        {
            if (forceRefresh)
            {
                var result = await sourceTask();
                await SetEntity(key, result, ts ?? Constants.Cache.DefaultTimespan);
                return result;
            }

            return await _entityCache.GetEntity<T>(key, sourceTask, ts ?? Constants.Cache.DefaultTimespan);
        }

        /// <summary>
        /// Tries the cache before loading
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> LoadEntity<T>(string key, string url, bool forceRefresh = false, TimeSpan? ts = null)
            where T: class, new()
        {
            var eRepo = Scope.Resolve<IGenericWebRepo<T>>();

            if (forceRefresh)
            {
                return await eRepo.Get(url);
            }
            return await GetEntity(key, () => eRepo.Get(url), ts);
        }

        public async Task<List<T>> LoadLinkEntitiesFromList<T, TLinks>(List<TLinks> links, string key, string rel, bool forceRefresh)
            where T : class, new()
            where TLinks : ListOfLinks
        {
            if (links == null)
            {
                return null;
            }

            key += _getHashLinksLinks(links);
            
            var cacheResult = await GetEntity<List<T>>(key);

            if (cacheResult != null && !forceRefresh)
            {
                return cacheResult;
            }

            var resultTasks = new List<Task<List<T>>>();

            foreach (var lol in links)
            {
                if (lol == null)
                {
                    continue;
                }
                resultTasks.Add(TaskThrottler.Get("LoadLinkEntitiesFromList", 15).Throttle(() => LoadLinkEntities<T>(lol, Guid.NewGuid().ToString(), rel, forceRefresh)));
            }

            var resultFromTasks = await Task.WhenAll(resultTasks);

            var listResult = resultFromTasks.ToList();

            var result = new List<T>();

            foreach (var l in listResult)
            {
                result.AddRange(l);
            }

            await SetEntity(key, result);

            return result;
        }
        public async Task<List<T>> LoadLinkEntities<T>(ListOfLinks links, string key, string rel, bool forceRefresh)
            where T : class, new()
        {
            if (links == null)
            {
                return null;
            }
            key += _getHashLinks(links);

            var cacheResult = await GetEntity<List<T>>(key);

            if (cacheResult != null && !forceRefresh)
            {
                return cacheResult;
            }

            var repo = Scope.Resolve<IGenericWebRepo<T>>();

            var resultTasks = new List<Task<T>>();

            if (links.Link == null)
            {
                return null;
            }

            foreach (var l in links.Link)
            {
                if (l.Rel != rel)
                {
                    continue;
                }

                var lClosure = l;

                lClosure.Href = lClosure.Href.Replace("sessionregistrations.", "sessionregistrations");//hardcode fix a bug in the arlo api

                resultTasks.Add(TaskThrottler.Get("GetLinkEntities", 15).Throttle(() => repo.Get(lClosure.Href)));
            }

            var resultFromTasks = await Task.WhenAll(resultTasks);

            var listResult = resultFromTasks.ToList();

            await SetEntity(key, listResult);

            return listResult;

        }

        public async Task<T> RecurseGetLinks<T>(ListOfLinks links, string key, string rel, bool doNext, bool forceRefresh)
            where T : ListOfLinks, new()
        {
            key += _getHashLinks(links);
            var cacheResult = await GetEntity<T>(key);

            if (cacheResult != null && !forceRefresh) 
            {
                return cacheResult;
            }

            var sessionUrl = links.FindLink(rel);

            if (sessionUrl == null)
            {
                return null;
            }

            var sessionsFromServer = await _recurseLinks<T>(sessionUrl, doNext);

            await SetEntity(key, sessionsFromServer);

            return sessionsFromServer;
        }

        async Task<T> _recurseLinks<T>(string sessionUrl, bool doNext)
            where T:ListOfLinks, new()
        {
            var repo = Scope.Resolve<IGenericWebRepo<T>>();

            var sessions = await repo.Get(sessionUrl);

            if (sessions == null)
            {
                return null;
            }

            if (!doNext)
            {
                return sessions;
            }

            var nextUrl = sessions.FindLink(Constants.Rel.Next);

            if (nextUrl == null)
            {
                return sessions;
            }

            var sessionsRecursed = await _recurseLinks<T>(nextUrl, true);

            if (sessionsRecursed?.Link == null)
            {
                return sessions;
            }

            sessions.Link.AddRange(sessionsRecursed.Link);

            return sessions;
        }

        string _getHashLinksLinks<TLinks>(List<TLinks> links)
            where TLinks:ListOfLinks
        {
            var list = new List<string>();

            foreach (var l in links)
            {
                var linksSelected = l?.Link?.Select(_ => _.Href).ToList();

                if (linksSelected == null || linksSelected.Count == 0)
                {
                    continue;
                }

                var s = string.Join("", linksSelected);

                var hash = HashHelper.Hash(Encoding.UTF8.GetBytes(s));
                list.Add(hash);
            }

            var sAll = string.Join("", list);

            return HashHelper.Hash(Encoding.UTF8.GetBytes(sAll));
        }

        string _getHashLinks(ListOfLinks links)
        {
            if(links == null)
            {
                return null;
            }
            var s = string.Join("", links.Link.Select(_ => _.Href));
            var hash = HashHelper.Hash(Encoding.UTF8.GetBytes(s));
            return hash;
        }
    }
}
