using System;
using System.Collections.Concurrent;

namespace Amo.Lib.RestClient
{
    /// <summary>
    /// HttpApi工厂缓存
    /// </summary>
    public abstract partial class HttpApi
    {
        /// <summary>
        /// 工厂字典
        /// </summary>
        private static readonly ConcurrentDictionary<string, HttpApiFactory> Factories = new ConcurrentDictionary<string, HttpApiFactory>();

        /// <summary>
        /// 注册各个Site的接口工厂
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="site">Site</param>
        /// <returns>接口的Factory</returns>
        public static HttpApiFactory<TInterface> Register<TInterface>(string site)
            where TInterface : class
        {
            Type interfaceType = typeof(TInterface);
            var httpApiFactory = new HttpApiFactory<TInterface>(site);
            return RegisterFactory(GetFactoryName(site, interfaceType), httpApiFactory);
        }

        /// <summary>
        /// 注册指定Api工厂
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="name">工厂名称</param>
        /// <param name="httpApiFactory">工厂实例</param>
        /// <returns>接口Factory</returns>
        public static TInterface RegisterFactory<TInterface>(string name, TInterface httpApiFactory)
            where TInterface : HttpApiFactory
        {
            if (httpApiFactory == null)
            {
                throw new ArgumentNullException(nameof(httpApiFactory));
            }

            if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (Factories.TryAdd(name, httpApiFactory) == true)
            {
                return httpApiFactory;
            }

            throw new InvalidOperationException($"不允许注册重复名称的工厂名称：{name}");
        }

        /// <summary>
        /// 获取指定Api工厂
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <param name="site">Site</param>
        /// <returns>Api工厂</returns>
        public static HttpApiFactory Resolve<TInterface>(string site)
            where TInterface : class
        {
            return Resolve(site, typeof(TInterface));
        }

        /// <summary>
        /// 获取指定Api工厂
        /// </summary>
        /// <param name="site">Site</param>
        /// <param name="type">类型</param>
        /// <returns>Api工厂</returns>
        public static HttpApiFactory Resolve(string site, Type type)
        {
            if (Factories.TryGetValue(GetFactoryName(site, type), out var factory) == true)
            {
                return factory;
            }

            throw new InvalidOperationException($"尚未Register(){site}.{type.FullName}的接口");
        }

        /// <summary>
        /// 获取Factory名字
        /// </summary>
        /// <param name="site">Site</param>
        /// <param name="type">类型</param>
        /// <returns>名字</returns>
        private static string GetFactoryName(string site, Type type)
        {
            return $"{site}-{type.FullName}";
        }
    }
}
