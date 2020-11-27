using Amo.Lib.RestClient.Defaults;
using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.Contexts
{
    /// <summary>
    /// Api的Context
    /// </summary>
    public class ApiActionContext
    {
        /// <summary>
        /// Json类型
        /// </summary>
        public static readonly string[] JsonAccept =
        {
            "application/json", "text/json", "text/x-json", "text/javascript"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiActionContext"/> class.
        /// 请求Api的上下文
        /// </summary>
        /// <param name="httpApiConfig">关联的HttpApiConfig</param>
        /// <param name="apiActionDescriptor">关联的ApiActionDescriptor</param>
        /// <param name="parameters">参数列表</param>
        public ApiActionContext(HttpApiConfig httpApiConfig, ApiActionDescriptor apiActionDescriptor, IReadOnlyList<ApiParameterDescriptor> parameters)
        {
            this.HttpApiConfig = httpApiConfig ?? throw new ArgumentNullException(nameof(httpApiConfig));
            this.ApiActionDescriptor = apiActionDescriptor ?? throw new ArgumentNullException(nameof(apiActionDescriptor));
            this.Parameters = parameters;
            this.RequestMessage = new HttpRequestMessage { RequestUri = apiActionDescriptor.Uri(), Method = apiActionDescriptor.Method };
        }

        /// <summary>
        /// 获取关联的HttpApiConfig
        /// </summary>
        public HttpApiConfig HttpApiConfig { get; }

        /// <summary>
        /// 获取关联的ApiActionDescriptor
        /// </summary>
        public ApiActionDescriptor ApiActionDescriptor { get; }

        /// <summary>
        /// 获取关联的HttpRequestMessage
        /// </summary>
        public HttpRequestMessage RequestMessage { get; }

        /// <summary>
        /// 获取Api的参数描述
        /// </summary>
        public IReadOnlyList<ApiParameterDescriptor> Parameters { get; protected set; }

        /// <summary>
        /// 执行Api方法
        /// </summary>
        /// <typeparam name="TResponse">返回类型</typeparam>
        /// <returns>执行结果</returns>
        public async Task<TResponse> ExecuteActionAsync<TResponse>()
        {
            this.ConfigureHeader(this.RequestMessage.Headers);
            this.ExecApiAttributes();
            try
            {
                return await this.SendAsync<TResponse>();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new Exception("HttpRequest Faild.");
        }

        /// <summary>
        /// 配置请求头的accept
        /// </summary>
        /// <param name="accept">请求头的accept</param>
        protected void ConfigureAccept(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> accept)
        {
            accept.Add(new MediaTypeWithQualityHeaderValue(JsonContent.MediaType));
        }

        /// <summary>
        /// 配置请求头
        /// </summary>
        /// <param name="headers">请求头</param>
        protected void ConfigureHeader(HttpRequestHeaders headers)
        {
            this.ConfigureAccept(headers.Accept);
            headers.Add("site", this.HttpApiConfig.Site);
        }

        /// <summary>
        /// 执行Api的所有Method特性的请求前参数载入
        /// </summary>
        private void ExecApiAttributes()
        {
            var apiAction = this.ApiActionDescriptor;

            // HttpAction几种类型需要在请求前处理赋值
            foreach (var actionAttribute in apiAction.Attributes.Where(p => p.IsMethod))
            {
                actionAttribute.BeforeRequest(this);
            }
        }

        /// <summary>
        /// 执行http请求
        /// </summary>
        /// <returns>请求结果</returns>
        private async Task<TResponse> SendAsync<TResponse>()
        {
            TResponse result;
            using (var cancellation = this.CreateLinkedTokenSource())
            {
                var completionOption = HttpCompletionOption.ResponseContentRead;

                var response = await this.HttpApiConfig.HttpClient.SendAsync(this.RequestMessage, completionOption, cancellation.Token).ConfigureAwait(false);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw GetFaildException(response);
                }

                try
                {
                    result = JsonExtensions.Deserialize<TResponse>(responseString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        /// <summary>
        /// 创建取消令牌源
        /// </summary>
        /// <returns>取消令牌</returns>
        private CancellationTokenSource CreateLinkedTokenSource()
        {
            return CancellationTokenSource.CreateLinkedTokenSource(CancellationToken.None);
        }

        private Exception GetFaildException(HttpResponseMessage response)
        {
            Exception exception = new Exception();
            if (response != null)
            {
                string infomation = this.RequestMessage.ToString();
                string parms = string.Empty;
                if (Parameters != null && Parameters.Count > 0)
                {
                    parms = "Parameters:{";
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        parms += Parameters[i].Name + ":" + Parameters[i].Value.ToJson().ToString() + ";";
                    }

                    parms += "}";
                }

                infomation += parms;

                exception = new Exception(infomation);
            }

            return exception;
        }
    }
}
