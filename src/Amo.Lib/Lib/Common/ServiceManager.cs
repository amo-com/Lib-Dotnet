using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amo.Lib
{
    public class ServiceManager
    {
        private static readonly ConcurrentDictionary<string, ServiceProvider> ProviderFactory = new ConcurrentDictionary<string, ServiceProvider>();

        /// <summary>
        /// 自动注册Services
        /// </summary>
        /// <param name="rootServices">默认的单例Services</param>
        /// <param name="sites">Site列表</param>
        /// <param name="nameSpaces">需要查找的命名空间列表</param>
        public static void RegisterServices(IServiceCollection rootServices, List<string> sites, List<string> nameSpaces)
        {
            // Root中的底层实例build一份,用于获取全局的ILog等,Copy给每个Site
            var rootProvider = rootServices.BuildServiceProvider();
            var logImpl = rootProvider.GetService<ILog>();

            List<Type> types = GetImplementationTypes(nameSpaces);

            // 注册全局的
            RegisterRootInterface(rootServices, types, logImpl);

            // site,注册每个site的(以site为作用域)
            sites?.ForEach(site =>
            {
                IServiceCollection services = new ServiceCollection()
                        .AddScoped<ISite, SiteFac>(fac => new SiteFac(site));

                if (logImpl != null)
                {
                    services.AddScoped(impl => logImpl);
                }

                RegisterSiteInterface(services, types, site, logImpl);

                var siteProvider = services.BuildServiceProvider();
                ProviderFactory.TryAdd(site, siteProvider);
            });
        }

        /// <summary>
        /// 自动注册Services
        /// </summary>
        /// <param name="rootServices">默认的单例Services</param>
        /// <param name="sites">Site列表</param>
        /// <param name="prefixs">需要查找的命名空间前缀</param>
        public static void RegisterServicesByPrefix(IServiceCollection rootServices, List<string> sites, List<string> prefixs)
        {
            var deps = DependencyContext.Default;
            if (prefixs == null)
            {
                prefixs = new List<string>();
            }

            prefixs.Add("Amo.");

            prefixs = prefixs.Distinct().ToList();

            List<string> nameSpaces = deps.CompileLibraries.Select(q => q.Name).Distinct().ToList().FindAll(q => StartWith(q, prefixs));

            // Root中的底层实例build一份,用于获取全局的ILog等,Copy给每个Site
            var rootProvider = rootServices.BuildServiceProvider();
            var logImpl = rootProvider.GetService<ILog>();

            List<Type> types = GetImplementationTypes(nameSpaces);

            // 注册全局的
            RegisterRootInterface(rootServices, types, logImpl);

            // site,注册每个site的(以site为作用域)
            sites?.ForEach(site =>
            {
                IServiceCollection services = new ServiceCollection()
                        .AddScoped<ISite, SiteFac>(fac => new SiteFac(site));

                if (logImpl != null)
                {
                    services.AddScoped(impl => logImpl);
                }

                RegisterSiteInterface(services, types, site, logImpl);

                var siteProvider = services.BuildServiceProvider();
                ProviderFactory.TryAdd(site, siteProvider);
            });
        }

        /// <summary>
        /// 获取Site作用域下的实例
        /// </summary>
        /// <typeparam name="TService">接口类型</typeparam>
        /// <param name="site">Site</param>
        /// <returns>实例</returns>
        public static TService GetSiteService<TService>(string site)
        {
            if (ProviderFactory.ContainsKey(site))
            {
                return ProviderFactory[site].GetService<TService>();
            }

            throw new Exception($"{site}未注册");
        }

        private static void RegisterRootInterface(IServiceCollection services, List<Type> implementationTypes, ILog log)
        {
            foreach (var implementationType in implementationTypes)
            {
                foreach (var interfaceType in implementationType.GetInterfaces())
                {
                    var autowiredAttribute = interfaceType.GetAttribute<AutowiredAttribute>(false);

                    // 注册全局的
                    if (autowiredAttribute != null && autowiredAttribute.ScopeType == Enums.ScopeType.Root)
                    {
                        log?.Info($"{interfaceType.FullName}-{implementationType.FullName}");
                        services.AddSingleton(interfaceType, implementationType);
                    }
                }
            }
        }

        /// <summary>
        /// 注册Site作用域的实例
        /// 遍历Class,再遍历Class的接口
        /// Class上如果有Sites属性,就需要和当前Site匹配,否则不注册,有伪目录这样的,每个Brand一个Class,注册时需要动态识别Site对应的Class
        /// 由于做了作用域隔离,Site的也要注册root的接口,否则访问不到
        /// </summary>
        /// <param name="services">作用域Service</param>
        /// <param name="implementationTypes">所有实现类(非接口的不需要注入)</param>
        /// <param name="site">当前Site</param>
        /// <param name="log">Log</param>
        private static void RegisterSiteInterface(IServiceCollection services, List<Type> implementationTypes, string site, ILog log)
        {
            // 遍历Class
            foreach (var implementationType in implementationTypes)
            {
                // 如果Class上有打Sites属性,并且Sites不为空,和当前Site比对,一致了才注册,否则不是当前Site的,跳过注册
                var sites = implementationType.GetAttribute<SitesAttribute>(false)?.Sites;
                if (sites != null && sites.Length > 0 && !sites.Contains(site))
                {
                    continue;
                }

                // 遍历Class依赖的接口
                foreach (var interfaceType in implementationType.GetInterfaces())
                {
                    var autowiredAttribute = interfaceType.GetAttribute<AutowiredAttribute>(false);

                    // 注册Site作用域的
                    if (autowiredAttribute != null && (autowiredAttribute.ScopeType == Enums.ScopeType.Root || autowiredAttribute.ScopeType == Enums.ScopeType.Site))
                    {
                        log?.Info($"{interfaceType.FullName}-{implementationType.FullName}");
                        services.AddScoped(interfaceType, implementationType);
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有实现类,排除抽象类和被覆盖类
        /// 为防止DI注册多个实例,被覆盖的类不做实例化,只实例化顶层类
        /// 覆盖示例:接口ITest,  Test: ITest, NewTest: Test
        /// Test被NewTest覆盖了,需在NewTest添加OverRide特性主动隐藏Test
        /// </summary>
        /// <param name="nameSpaces">需要检测的命名空间</param>
        /// <returns>实现类</returns>
        private static List<Type> GetImplementationTypes(List<string> nameSpaces)
        {
            List<Type> implementationTypes = new List<Type>();

            nameSpaces.ForEach(nameSpace =>
            {
                var assembly = Assembly.Load(nameSpace);

                // 所有非抽象类
                List<Type> currentTypes = assembly.GetTypes().Where(type => !type.GetTypeInfo().IsAbstract).ToList();

                if (currentTypes != null)
                {
                    implementationTypes.AddRange(currentTypes);
                }
            });

            // 所有被覆盖的类
            List<Type> overRideTypes = implementationTypes.FindAll(q => q.GetAttribute<OverRideAttribute>(false) != null)?.Select(q => q.BaseType).ToList();

            // 移除被覆盖的类
            if (overRideTypes != null)
            {
                implementationTypes = implementationTypes.FindAll(q => !overRideTypes.Contains(q));
            }

            return implementationTypes;
        }

        private static bool StartWith(string name, List<string> prefixs)
        {
            if (string.IsNullOrEmpty(name) || prefixs == null || prefixs.Count == 0)
            {
                return true;
            }

            foreach (var prefix in prefixs)
            {
                if (name.StartsWith(prefix))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
