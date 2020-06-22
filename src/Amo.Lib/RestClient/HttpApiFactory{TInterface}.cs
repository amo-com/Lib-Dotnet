using System;

namespace Amo.Lib.RestClient
{
    /// <summary>
    /// 泛型HttpApi工厂
    /// </summary>
    /// <typeparam name="TInterface">接口类型</typeparam>
    public class HttpApiFactory<TInterface> : HttpApiFactory
    where TInterface : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiFactory{TInterface}"/> class.
        /// HttpApi创建工厂
        /// </summary>
        /// <param name="site">Site</param>
        public HttpApiFactory(string site)
            : base(site, typeof(TInterface))
        {
        }

        /// <summary>
        /// 配置HttpApiConfig
        /// </summary>
        /// <param name="options">配置委托</param>
        /// <returns>接口的Factory</returns>
        public new HttpApiFactory<TInterface> ConfigureHttpApiConfig(Action<HttpApiConfig> options)
        {
            return base.ConfigureHttpApiConfig(options) as HttpApiFactory<TInterface>;
        }
    }
}
