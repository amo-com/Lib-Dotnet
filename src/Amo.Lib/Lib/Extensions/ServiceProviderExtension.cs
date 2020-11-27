using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib.Extensions
{
    public static class ServiceProviderExtension
    {
        public static async Task<List<string>> GetServicesWithCache(this IDiscoveryClient discoveryClient, string serviceName, IDistributedCache distributedCache = null, DistributedCacheEntryOptions cacheEntryOptions = null)
        {
            if (distributedCache != null)
            {
                var cacheData = await distributedCache.GetAsync(serviceName);
                if (cacheData != null && cacheData.Length > 0)
                {
                    return DeserializeFromCache<List<string>>(cacheData);
                }
            }

            var services = await discoveryClient.GetHealthServices(serviceName);
            if (distributedCache != null && services != null && services.Count > 0)
            {
                byte[] obj = SerializeForCache(services);
                await distributedCache.SetAsync(serviceName, obj, cacheEntryOptions ?? new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
            }

            return services;
        }

        private static T DeserializeFromCache<T>(byte[] data)
            where T : class
        {
            using (var stream = new MemoryStream(data))
            {
                return new BinaryFormatter().Deserialize(stream) as T;
            }
        }

        private static byte[] SerializeForCache(object data)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, data);
                return stream.ToArray();
            }
        }
    }
}
