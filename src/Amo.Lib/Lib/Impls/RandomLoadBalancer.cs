using Amo.Lib.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib.Impls
{
    public class RandomLoadBalancer : ILoadBalancer
    {
        protected static readonly Random Random = new Random();
        protected readonly IDiscoveryClient discoveryClient;
        protected readonly IDistributedCache distributedCache;
        protected readonly DistributedCacheEntryOptions cacheEntryOptions;

        public RandomLoadBalancer(IDiscoveryClient discoveryClient, IDistributedCache distributedCache = null, DistributedCacheEntryOptions cacheEntryOptions = null)
        {
            this.discoveryClient = discoveryClient;
            this.distributedCache = distributedCache;
            this.cacheEntryOptions = cacheEntryOptions;
        }

        public async Task<string> ResolveServiceInstance(string serviceName)
        {
            string url = string.Empty;
            var healthInstances = await discoveryClient.GetServicesWithCache(serviceName, distributedCache, cacheEntryOptions);
            if (healthInstances != null && healthInstances.Count > 0)
            {
                url = healthInstances[Random.Next(healthInstances.Count)];
            }

            return url;
        }
    }
}
