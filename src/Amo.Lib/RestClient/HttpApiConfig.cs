using System;
using System.Net.Http;

namespace Amo.Lib.RestClient
{
    /// <summary>
    /// Api的Config配置
    /// </summary>
    public class HttpApiConfig : IDisposable
    {
        private string host;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiConfig"/> class.
        /// Http接口的配置项
        /// </summary>
        public HttpApiConfig()
            : this(new HttpClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiConfig"/> class.
        /// Http接口的配置项
        /// </summary>
        /// <param name="handler">HTTP消息处理程序</param>
        /// <param name="disposeHandler">用Dispose方法时，是否也Dispose handler</param>
        public HttpApiConfig(HttpMessageHandler handler, bool disposeHandler = false)
            : this(new HttpClient(handler, disposeHandler))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiConfig"/> class.
        /// Http接口的配置项
        /// </summary>
        /// <param name="httpClient">外部HttpClient实例</param>
        public HttpApiConfig(HttpClient httpClient)
        {
            this.HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public string Host
        {
            get => host;
            set
            {
                host = value;
                HttpHost = new Uri(value);
            }
        }

        public string Site { get; set; }

        /// <summary>
        /// 获取HttpClient实例
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// 获取或设置Http服务完整主机域名
        /// 例如http://www.webapiclient.com
        /// 设置了HttpHost值，HttpHostAttribute将失效
        /// </summary>
        public Uri HttpHost
        {
            get => this.HttpClient.BaseAddress;
            private set => this.HttpClient.BaseAddress = value ?? throw new ArgumentNullException(nameof(HttpHost));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.HttpClient.Dispose();
        }
    }
}
