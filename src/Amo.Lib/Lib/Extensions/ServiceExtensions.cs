using Amo.Lib.Attributes;
using Amo.Lib.Intercept;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Amo.Lib.Extensions
{
    public static class ServiceExtensions
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

        public static void AddProxiedScoped(this IServiceCollection services, Type interfaceType, Type implementationType)
        {
            // services.AddSingleton(implementationType);
            services.Add(new ServiceDescriptor(implementationType, implementationType, ServiceLifetime.Singleton));

            Func<IServiceProvider, object> factory = (serviceProvider) =>
             {
                 var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                 var actual = serviceProvider.GetRequiredService(implementationType);

                 // 获取本接口用到的拦截器
                 var attrInterceptAttribute = interfaceType.GetAttribute<InterceptsAttribute>(true);
                 var interceptTypes = attrInterceptAttribute?.Types.ToList().FindAll(q => q.GetInterface(typeof(IInterceptor).Name) != null).ToList();

                 IInterceptor[] interceptors = GetInterceptors(serviceProvider, interfaceType).ToArray();

                 return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceType, actual, interceptors);
             };

            services.Add(new ServiceDescriptor(interfaceType, factory, ServiceLifetime.Singleton));
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

        /// <summary>
        /// 获取接口的所有Interceptor特性
        /// </summary>
        /// <param name="serviceProvider">DI容器</param>
        /// <param name="interfaceType">接口实例</param>
        /// <returns>Interceptors</returns>
        private static List<IInterceptor> GetInterceptors(IServiceProvider serviceProvider, Type interfaceType)
        {
            List<IInterceptor> interceptors = new List<IInterceptor>();

            // 本接口需要注册的拦截器
            var interceptsAttribute = interfaceType.GetAttribute<InterceptsAttribute>(true);
            var types = interceptsAttribute?.Types?.ToList();

            if (types != null && types.Count() > 0)
            {
                var interceptorImpls = GetInterceptors<IInterceptor>(serviceProvider, types);
                if (interceptorImpls != null)
                {
                    interceptors.AddRange(interceptorImpls);
                }

                var asyncInterceptorImpls = GetInterceptors<IAsyncInterceptor>(serviceProvider, types);
                if (asyncInterceptorImpls != null)
                {
                    interceptors.AddRange(asyncInterceptorImpls.Select(q => new AsyncInterceptorMapper(q)).ToList());
                }
            }

            return interceptors;
        }

        private static List<TInterceptor> GetInterceptors<TInterceptor>(IServiceProvider serviceProvider, List<Type> interceptorImplTypes)
        {
            List<TInterceptor> interceptors = new List<TInterceptor>();

            if (interceptorImplTypes != null && interceptorImplTypes.Count() > 0)
            {
                var interceptorType = typeof(TInterceptor);
                var interceptorImpls = serviceProvider.GetServices<TInterceptor>().ToList();

                var interceptorRefs = interceptorImplTypes.FindAll(q => q.GetInterfaces().Contains(interceptorType));
                if (interceptorImpls != null && interceptorImpls.Count() > 0
                    && interceptorRefs != null && interceptorRefs.Count() > 0)
                {
                    var refImpls = interceptorImpls.FindAll(q => interceptorRefs.Contains(q.GetType())).ToList();
                    if (refImpls != null)
                    {
                        interceptors.AddRange(refImpls);
                    }
                }
            }

            return interceptors;
        }
    }
}
