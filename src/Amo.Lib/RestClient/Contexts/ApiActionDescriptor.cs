using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Amo.Lib.RestClient.Contexts
{
    /// <summary>
    /// 表示请求Api描述
    /// </summary>
    public class ApiActionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiActionDescriptor"/> class.
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="methodInfo">Method</param>
        public ApiActionDescriptor(string host, MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(MethodInfo));
            }

            var actionAttributes = methodInfo
                .FindDeclaringAttributes<IApiActionAttribute>(true)
                .Distinct(new MultiplableComparer<IApiActionAttribute>())
                .OrderBy(item => item.OrderIndex)
                .ToReadOnlyList();

            this.Host = host;
            this.Member = methodInfo;
            this.Name = methodInfo.Name;
            this.Attributes = actionAttributes;

            // this.ReturnType = method.ReflectedType;
            this.Parameters = methodInfo.GetParameters().Select(p => new ApiParameterDescriptor(p)).ToReadOnlyList();
            this.Uri = () => Url;
        }

        public ApiActionDescriptor(string host, MethodInfo methodInfo, HttpMethod method)
            : this(host, methodInfo)
        {
            this.Method = method;
        }

        protected ApiActionDescriptor()
        {
        }

        /// <summary>
        /// HttpGet、HttpPost....
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// http://localhost:8080
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// api/vehicle
        /// </summary>
        public string RoutePrefix { get; set; }

        /// <summary>
        /// make-list
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// http://localhost:8080/api/vehicle/make-list
        /// </summary>
        public Uri Url { get; set; }

        // 默认:() => Url
        public Func<Uri> Uri { get; set; }

        /// <summary>
        /// 获取Api名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 获取关联的方法信息
        /// </summary>
        public MethodInfo Member { get; protected set; }

        /// <summary>
        /// 获取Api关联的特性
        /// </summary>
        public IReadOnlyList<IApiActionAttribute> Attributes { get; protected set; }

        /// <summary>
        /// 获取Api的参数描述
        /// </summary>
        public IReadOnlyList<ApiParameterDescriptor> Parameters { get; protected set; }

        /// <summary>
        /// 是否允许重复的特性比较器
        /// </summary>
        private class MultiplableComparer<TAttributeMultiplable> : IEqualityComparer<TAttributeMultiplable>
            where TAttributeMultiplable : IApiActionAttribute
        {
            /// <summary>
            /// 是否相等
            /// </summary>
            /// <param name="x">第一个 AttributeMultiplable</param>
            /// <param name="y">第二个 AttributeMultiplable</param>
            /// <returns>是否允许重复</returns>
            public bool Equals(TAttributeMultiplable x, TAttributeMultiplable y)
            {
                // 如果其中一个不允许重复，返回true将y过滤
                return x.AllowMultiple == false || y.AllowMultiple == false;
            }

            /// <summary>
            /// 获取哈希码
            /// </summary>
            /// <param name="obj">AttributeMultiplable</param>
            /// <returns>HashCode</returns>
            public int GetHashCode(TAttributeMultiplable obj)
            {
                return obj.GetType().GetHashCode();
            }
        }
    }
}
