using Amo.Lib.CoreApi.Models;
using Amo.Lib.Impls;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.CoreApi.Common
{
    public static class RegisterExtensions
    {
        private const string CONSUL = "consul";
        public static void ApplyServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsulOptions>(configuration.GetSection(CONSUL));
            services.TryAddSingleton(q =>
            {
                var options = q.GetRequiredService<IOptions<ConsulOptions>>();
                int ttl = options?.Value.CacheTTL ?? 30;
                return new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(ttl) };
            });
            services.AddSingleton(q =>
            {
                var options = q.GetRequiredService<IOptions<ConsulOptions>>();
                return ConsulClientFactory.CreateClient(options?.Value.ConsulAddress, options?.Value.Datacenter);
            });
            services.AddSingleton<IServiceRegistry, ConsulServiceRegistry>();
            services.AddSingleton<IDiscoveryClient, ConsulDiscoveryClient>();
            services.AddSingleton<ILoadBalancer, RandomLoadBalancer>();
            services.AddHealthChecks();
            services.AddDistributedMemoryCache();
        }
    }
}
