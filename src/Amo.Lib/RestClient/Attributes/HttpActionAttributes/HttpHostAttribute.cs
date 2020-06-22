using Amo.Lib.RestClient.Contexts;
using System;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// Host标记传参
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class HttpHostAttribute : ApiActionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHostAttribute"/> class.
        /// 请求服务的根路径
        /// 例如http://www.abc.com
        /// </summary>
        /// <param name="host">请求完整绝对根路径</param>
        public HttpHostAttribute(string host)
        {
            this.Host = host;
        }

        /// <summary>
        /// 获取根路径
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// 获取顺序排序索引
        /// int.MinValue
        /// </summary>
        public override int OrderIndex
        {
            get => int.MinValue + 1;
        }

        /// <summary>
        /// 处理Host信息
        /// </summary>
        /// <param name="descriptor">上下文环境</param>
        public override void Init(ApiActionDescriptor descriptor)
        {
            // 优先选取手动配置的host,获取不到时才使用类上配置的默认值
            if (descriptor.Host == null)
            {
                descriptor.Host = this.Host;
            }
        }

        public override void BeforeRequest(ApiActionContext context)
        {
        }
    }
}
