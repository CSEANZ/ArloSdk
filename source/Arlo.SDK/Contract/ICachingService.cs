using System;
using System.Threading.Tasks;

namespace Arlo.SDK.Contract
{
    public interface ICachingService
    {
        Task SetEntity<T>(string key, T entity, TimeSpan? ts = null)
            where T:class, new();

        Task<T> GetEntity<T>(string key)
            where T : class, new();
    }
}