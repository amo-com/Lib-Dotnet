using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient
{
    /// <summary>
    /// HttpApi工厂
    /// </summary>
    public class HttpApiFactory
    {
        /// <summary>
        /// HttpApiConfig的配置委托
        /// </summary>
        private Action<HttpApiConfig> configOptions;
        private Dictionary<string, ApiActionDescriptor> methodDescriptors = new Dictionary<string, ApiActionDescriptor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiFactory"/> class.
        /// </summary>
        /// <param name="site">Site</param>
        /// <param name="interfaceType">接口类型</param>
        public HttpApiFactory(string site, Type interfaceType)
        {
            this.Site = site;
            this.InterfaceType = interfaceType;
        }

        /// <summary>
        /// 接口类型
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// 网站
        /// </summary>
        private string Site { get; }

        private HttpApiConfig HttpApiConfig { get; set; }

        /// <summary>
        /// 配置HttpApiConfig
        /// </summary>
        /// <param name="options">配置委托</param>
        /// <returns>HttpApiFactory</returns>
        public HttpApiFactory ConfigureHttpApiConfig(Action<HttpApiConfig> options)
        {
            this.configOptions = options;
            return this;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            HttpApiConfig = new HttpApiConfig() { Site = this.Site };
            if (this.configOptions != null)
            {
                this.configOptions.Invoke(HttpApiConfig);
            }

            LoadMethods();
        }

        /// <summary>
        /// 获取当前方法的Request结果
        /// </summary>
        /// <typeparam name="TResponse">TResponse</typeparam>
        /// <param name="args">参数列表</param>
        /// <param name="memberName">方法名</param>
        /// <returns>请求结果TResponse</returns>
        public async Task<TResponse> Request<TResponse>(object[] args, [CallerMemberName] string memberName = null)
        {
            if (methodDescriptors.TryGetValue(memberName, out ApiActionDescriptor apiDescriptor) == true)
            {
                if (apiDescriptor.Parameters.Count != args.Length)
                {
                    throw new Exception($"参数丢失:{apiDescriptor.Parameters.Count}-{args.Length}");
                }

                IReadOnlyList<ApiParameterDescriptor> parameters = apiDescriptor.Parameters.Select((p, i) => p.Clone(args[i])).ToReadOnlyList();
                ApiActionContext apiActionContext = new ApiActionContext(HttpApiConfig, apiDescriptor, parameters);
                try
                {
                    return await apiActionContext.ExecuteActionAsync<TResponse>();
                }
                catch
                {
                    return default(TResponse);
                }
            }

            throw new InvalidOperationException($"当前方法未初始化：{memberName}");
        }

        /// <summary>
        /// 加载接口的所有Method
        /// </summary>
        private void LoadMethods()
        {
            // 获取所有Method
            var methods = this.InterfaceType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                // 获取Method的所有自定义Action扩展特性,并进行初始化,路由也在Descriptor中处理
                ApiActionDescriptor descriptor = new ApiActionDescriptor(HttpApiConfig.Host, method);
                foreach (var actionAttribute in descriptor.Attributes)
                {
                    actionAttribute.Init(descriptor);
                }

                if (methodDescriptors.ContainsKey(method.Name))
                {
                    throw new Exception($"存在同名方法{method.Name}");
                }

                methodDescriptors.Add(method.Name, descriptor);
            }
        }
    }
}
