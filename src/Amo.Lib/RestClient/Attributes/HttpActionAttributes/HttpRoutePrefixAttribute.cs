using Amo.Lib.RestClient.Contexts;
using System;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// Api路由前缀
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class HttpRoutePrefixAttribute : ApiActionAttribute
    {
        public HttpRoutePrefixAttribute(string routePrefix)
        {
            this.RoutePrefix = routePrefix;
        }

        public string RoutePrefix { get; }

        /// <summary>
        /// 获取顺序排序索引
        /// int.MinValue
        /// </summary>
        public override int OrderIndex
        {
            get => int.MinValue + 2;
        }

        public override void Init(ApiActionDescriptor descriptor)
        {
            descriptor.RoutePrefix = this.RoutePrefix;
        }

        public override void BeforeRequest(ApiActionContext context)
        {
        }
    }
}
