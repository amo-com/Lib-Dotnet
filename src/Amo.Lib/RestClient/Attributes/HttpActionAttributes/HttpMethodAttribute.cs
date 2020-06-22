using Amo.Lib.RestClient.Contexts;
using System;
using System.Net.Http;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 表示http请求方法描述特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HttpMethodAttribute : ApiActionAttribute
    {
        private static readonly char SplitChar = '/';
        private static readonly string SplitStr = "/";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// http请求方法描述特性
        /// </summary>
        /// <param name="method">请求方法</param>
        public HttpMethodAttribute(string method)
            : this(new HttpMethod(method))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// http请求方法描述特性
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="path">请求绝对或相对路径</param>
        public HttpMethodAttribute(string method, string path)
            : this(new HttpMethod(method), path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// http请求方法描述特性
        /// </summary>
        /// <param name="method">请求方法</param>
        protected HttpMethodAttribute(HttpMethod method)
            : this(method, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMethodAttribute"/> class.
        /// http请求方法描述特性
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="path">请求绝对或相对路径</param>
        protected HttpMethodAttribute(HttpMethod method, string path)
        {
            this.Method = method;
            this.Path = path;
        }

        /// <summary>
        /// Method标记
        /// </summary>
        public override bool IsMethod => true;

        /// <summary>
        /// 获取请求方法
        /// </summary>
        public HttpMethod Method { get; }

        /// <summary>
        /// 获取请求相对路径
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// 获取顺序排序索引
        /// 优先级最高
        /// </summary>
        public override int OrderIndex
        {
            get => int.MinValue + 3;
        }

        /// <summary>
        /// method处理,进行上下文绑定
        /// </summary>
        /// <param name="descriptor">上下文</param>
        public override void Init(ApiActionDescriptor descriptor)
        {
            descriptor.Method = this.Method;
            descriptor.Route = this.Path;
            descriptor.Url = GetUri(descriptor.Host, descriptor.RoutePrefix, descriptor.Route);
        }

        /// <summary>
        /// 请求之前配置参数
        /// </summary>
        /// <param name="context">上下文</param>
        public override void BeforeRequest(ApiActionContext context)
        {
            // 拼接参数
            var uri = context.RequestMessage.RequestUri;
            if (uri == null)
            {
                throw new Exception($"未配置HttpHost，无法使用参数");
            }

            foreach (ApiParameterDescriptor apiParameter in context.Parameters)
            {
                apiParameter.ParameterAttribute?.BeforeRequest(context, apiParameter);
            }
        }

        /// <summary>
        /// 拼接Uri信息
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="routePrefix">路由前缀</param>
        /// <param name="route">路由</param>
        /// <returns>Uri</returns>
        private Uri GetUri(string host, string routePrefix, string route)
        {
            string url = host ?? throw new Exception("未配置Host");
            url = this.CombineRoute(url, routePrefix);
            url = this.CombineRoute(url, route);

            return new Uri(url);
        }

        private string CombineRoute(string left, string right)
        {
            if (!string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right))
            {
                if (left.EndsWith(SplitStr) || right.StartsWith(SplitStr))
                {
                    return $"{left.TrimEnd(SplitChar)}/{right.TrimStart(SplitChar)}";
                }

                return $"{left}/{right}";
            }
            else if (!string.IsNullOrEmpty(left))
            {
                return left;
            }

            return right;
        }
    }
}
