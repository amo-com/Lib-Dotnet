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
        private static readonly ConcurrentDictionary<string, IServiceCollection> ServiceCollectionFactory = new ConcurrentDictionary<string, IServiceCollection>();

        /// <summary>
        /// 注册DI实例(注册完需调用BuildServices生成实例)
        /// </summary>
        /// <param name="services">IServiceCollection实例</param>
        /// <param name="scopeds">作用域列表(为每个作用域都要注册一份实例),Scope不可以是null或者空值</param>
        /// <param name="log">ILog实例</param>
        /// <param name="nameSpaces">需要检索的组件命名空间列表</param>
        /// <param name="prefixs">需要检索的组件前缀列表(与nameSpaces二选一,优先使用nameSpaces):Amo.</param>
        public static void RegisterServices(IServiceCollection services, List<string> scopeds, ILog log, List<string> nameSpaces, List<string> prefixs = null)
        {
            RegisterRootServices(services, log, nameSpaces, prefixs);
            RegisterScopedsServices(scopeds, log, nameSpaces, prefixs);
        }

        /// <summary>
        /// 注册全局单一的实例(注册完需调用BuildServices生成实例)
        /// </summary>
        /// <param name="services">IServiceCollection实例</param>
        /// <param name="log">ILog实例</param>
        /// <param name="nameSpaces">需要检索的组件命名空间列表</param>
        /// <param name="prefixs">需要检索的组件前缀列表(与nameSpaces二选一,优先使用nameSpaces):Amo.</param>
        public static void RegisterRootServices(IServiceCollection services, ILog log, List<string> nameSpaces, List<string> prefixs = null)
        {
            if (services == null)
            {
                return;
            }

            List<Type> types = GetImplementationTypes(nameSpaces, prefixs);

            var resultTypes = RemoveOverRideTypes(types, null);

            // 注册全局的
            RegisterRootInterface(services, resultTypes, log);
        }

        /// <summary>
        /// 注册作用域的实例(注册完需调用BuildServices生成实例)
        /// </summary>
        /// <param name="scopeds">作用域列表(为每个作用域都要注册一份实例),Scope不可以是null或者空值</param>
        /// <param name="log">ILog实例</param>
        /// <param name="nameSpaces">需要检索的组件命名空间列表</param>
        /// <param name="prefixs">需要检索的组件前缀列表(与nameSpaces二选一,优先使用nameSpaces):Amo.</param>
        public static void RegisterScopedsServices(List<string> scopeds, ILog log, List<string> nameSpaces, List<string> prefixs = null)
        {
            if (scopeds == null || scopeds.Count == 0)
            {
                return;
            }

            List<Type> types = GetImplementationTypes(nameSpaces, prefixs);

            scopeds = scopeds.FindAll(q => !string.IsNullOrEmpty(q)).Distinct().ToList();

            // site,注册每个site的(以site为作用域)
            scopeds?.ForEach(scoped =>
            {
                bool exist = ServiceCollectionFactory.TryGetValue(scoped, out IServiceCollection services);
                if (!exist)
                {
                    services = new ServiceCollection().AddSingleton<IScoped, ScopedFac>(fac => new ScopedFac(scoped));
                    ServiceCollectionFactory.TryAdd(scoped, services);
                }

                var resultTypes = RemoveOverRideTypes(types, scoped);
                RegisterSiteInterface(services, resultTypes, scoped, log);
            });
        }

        /// <summary>
        /// 注册作用域的实例(注册完需调用BuildServices生成实例)
        /// </summary>
        /// <param name="scoped">,Scope不可以是null或者空值</param>
        /// <param name="log">ILog实例</param>
        /// <param name="nameSpaces">需要检索的组件命名空间列表</param>
        /// <param name="prefixs">需要检索的组件前缀列表(与nameSpaces二选一,优先使用nameSpaces):Amo.</param>
        public static void RegisterScopedServices(string scoped, ILog log, List<string> nameSpaces, List<string> prefixs = null)
        {
            if (string.IsNullOrEmpty(scoped))
            {
                return;
            }

            List<Type> types = GetImplementationTypes(nameSpaces, prefixs);

            bool exist = ServiceCollectionFactory.TryGetValue(scoped, out IServiceCollection services);
            if (!exist)
            {
                services = new ServiceCollection().AddSingleton<IScoped, ScopedFac>(fac => new ScopedFac(scoped));
                ServiceCollectionFactory.TryAdd(scoped, services);
            }

            var resultTypes = RemoveOverRideTypes(types, scoped);
            RegisterSiteInterface(services, resultTypes, scoped, log);
        }

        /// <summary>
        /// 所有未Build过的Scoped对应的ServiceCollection生成ServiceProvider
        /// </summary>
        public static void BuildServices()
        {
            foreach (string scoped in ServiceCollectionFactory.Keys)
            {
                if (ProviderFactory.ContainsKey(scoped))
                {
                    continue;
                }

                ProviderFactory.TryAdd(scoped, ServiceCollectionFactory[scoped].BuildServiceProvider());
            }
        }

        public static (bool serviceExist, bool providerExist) ExistScoped(string scoped)
        {
            bool serviceExist = ServiceCollectionFactory.ContainsKey(scoped);
            bool providerExist = ProviderFactory.ContainsKey(scoped);

            return (serviceExist, providerExist);
        }

        /// <summary>
        /// 获取作用域下的实例
        /// </summary>
        /// <typeparam name="TService">接口类型</typeparam>
        /// <param name="scoped">作用域</param>
        /// <returns>实例</returns>
        public static TService GetService<TService>(string scoped)
        {
            if (ProviderFactory.ContainsKey(scoped))
            {
                return ProviderFactory[scoped].GetService<TService>();
            }

            throw new Exception($"服务Scoped({scoped})未注册");
        }

        /// <summary>
        /// 获取作用域下的实例
        /// </summary>
        /// <typeparam name="TService">接口类型</typeparam>
        /// <param name="scoped">作用域</param>
        /// <returns>实例</returns>
        public static TService GetRequiredService<TService>(string scoped)
        {
            if (ProviderFactory.ContainsKey(scoped))
            {
                return ProviderFactory[scoped].GetRequiredService<TService>();
            }

            throw new Exception($"服务Scoped({scoped})未注册");
        }

        internal static void RegisterRootInterface(IServiceCollection services, List<Type> implementationTypes, ILog log)
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
        /// <param name="scoped">当前Site</param>
        /// <param name="log">Log</param>
        internal static void RegisterSiteInterface(IServiceCollection services, List<Type> implementationTypes, string scoped, ILog log)
        {
            if (services == null
                || implementationTypes == null || implementationTypes.Count == 0)
            {
                return;
            }

            // 遍历Class
            foreach (var implementationType in implementationTypes)
            {
                // 如果Class上有打Sites属性,并且Sites不为空,和当前Site比对,一致了才注册,否则不是当前Site的,跳过注册
                var sites = implementationType.GetAttribute<SitesAttribute>(false)?.Sites;
                if (sites != null && sites.Length > 0 && !sites.Contains(scoped))
                {
                    continue;
                }

                // 遍历Class依赖的接口
                foreach (var interfaceType in implementationType.GetInterfaces())
                {
                    var autowiredAttribute = interfaceType.GetAttribute<AutowiredAttribute>(false);

                    // 注册Site作用域的
                    if (autowiredAttribute != null && (autowiredAttribute.ScopeType == Enums.ScopeType.Root || autowiredAttribute.ScopeType == Enums.ScopeType.Scoped))
                    {
                        log?.Info($"{scoped}-{interfaceType.FullName}-{implementationType.FullName}");
                        services.AddSingleton(interfaceType, implementationType);
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
        /// <param name="nameSpaces">需要检索的组件命名空间列表</param>
        /// <param name="prefixs">需要检索的组件前缀列表(与nameSpaces二选一,优先使用nameSpaces)</param>
        /// <returns>实现类</returns>
        internal static List<Type> GetImplementationTypes(List<string> nameSpaces, List<string> prefixs)
        {
            List<Type> implementationTypes = new List<Type>();

            if ((nameSpaces == null || nameSpaces.Count == 0)
                && (prefixs == null || prefixs.Count == 0))
            {
                return implementationTypes;
            }

            if (nameSpaces == null)
            {
                nameSpaces = new List<string>();
            }

            if (prefixs != null && prefixs.Count > 0)
            {
                var deps = DependencyContext.Default;
                prefixs = prefixs.Distinct().ToList();

                List<string> preNameSpaces = deps.CompileLibraries.Select(q => q.Name).Distinct().ToList().FindAll(q => StartWith(q, prefixs));
                if (preNameSpaces != null)
                {
                    nameSpaces.AddRange(preNameSpaces);
                }
            }

            nameSpaces.Add("Amo.Lib");
            nameSpaces = nameSpaces?.Distinct().ToList();
            nameSpaces?.ForEach(nameSpace =>
            {
                try
                {
                    var assembly = Assembly.Load(nameSpace);

                    // 所有非抽象类
                    List<Type> currentTypes = assembly.GetTypes().Where(type => !type.GetTypeInfo().IsAbstract).ToList();

                    if (currentTypes != null)
                    {
                        implementationTypes.AddRange(currentTypes);
                    }
                }
                catch
                {
                }
            });

            // 所有被覆盖的类
            // List<Type> overRideTypes = implementationTypes.FindAll(q => q.GetAttribute<OverRideAttribute>(false) != null)?.Select(q => q.BaseType).ToList();

            // 所有被屏蔽的类
            List<Type> obsoleteTypes = implementationTypes.FindAll(q => q.GetAttribute<ObsoleteAttribute>(false) != null);

            // List<Type> removeTypes = overRideTypes?.Union(obsoleteTypes)?.ToList();
            var removeTypes = obsoleteTypes;

            // 移除被覆盖的类和被屏蔽的类
            if (removeTypes != null)
            {
                implementationTypes = implementationTypes.FindAll(q => !removeTypes.Contains(q));
            }

            return implementationTypes;
        }

        internal static bool StartWith(string name, List<string> prefixs)
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

        /// <summary>
        /// 移除OverRide标记的基类或者本类
        /// </summary>
        /// <param name="types">Type列表</param>
        /// <param name="scoped">作用域,Root的传null</param>
        /// <returns>处理后的类型列表</returns>
        internal static List<Type> RemoveOverRideTypes(List<Type> types, string scoped)
        {
            if (types == null)
            {
                return new List<Type>();
            }

            // 所有被覆盖的类
            List<Type> overRideTypes = types.FindAll(q => q.GetAttribute<OverRideAttribute>(false) != null);

            List<Type> removeTypes = new List<Type>();

            if (scoped == null)
            {
                removeTypes = overRideTypes.Select(q => q.BaseType).ToList();
            }
            else
            {
                overRideTypes?.ForEach(q =>
                {
                    var overRideAttr = q.GetAttribute<OverRideAttribute>(false);
                    var attrSites = overRideAttr.Sites;

                    // 没有指定Scoped或者指定的Scoped包含当前Scoped,则移除基类,保留本类
                    if (attrSites == null || attrSites.Length == 0 || attrSites.Contains(scoped))
                    {
                        removeTypes.Add(q.BaseType);
                    }

                    // 否则移除本类,保留基类,覆盖的实现中不包含当前Site,当前Site用基类的
                    else
                    {
                        removeTypes.Add(q);
                    }
                });
            }

            var siteTypes = types.FindAll(q => !removeTypes.Contains(q));

            return siteTypes;
        }
    }
}
