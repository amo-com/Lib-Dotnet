using Amo.Lib.CoreApi.Models;
using Amo.Lib.Impls;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Amo.Lib.CoreApi.Common
{
    public static class RegisterExtensions
    {
        public static void ApplyServices(this IServiceCollection services, IConfiguration configuration)
        {
            bool enableRegister = configuration.GetValue<bool>(ApiCommon.Appsetting.EnableRegister);
            bool enableDiscover = configuration.GetValue<bool>(ApiCommon.Appsetting.EnableDiscover);
            if (!enableRegister && !enableDiscover)
            {
                return;
            }

            services.Configure<ConsulOptions>(configuration.GetSection(ApiCommon.Appsetting.ConsulSection));
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

            if (enableRegister)
            {
                services.AddSingleton<IServiceRegistry, ConsulServiceRegistry>();
                services.AddHealthChecks();
            }

            if (enableDiscover)
            {
                services.AddSingleton<IDiscoveryClient, ConsulDiscoveryClient>();
                services.AddSingleton<ILoadBalancer, RandomLoadBalancer>();
                services.AddDistributedMemoryCache();
            }
        }
    }
}
