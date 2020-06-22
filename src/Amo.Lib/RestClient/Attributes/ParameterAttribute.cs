using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 参数类型,Query,Header,Body等
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public abstract class ParameterAttribute : Attribute, IParameterAttribute
    {
        /// <summary>
        /// 初始化,准备数据
        /// </summary>
        /// <param name="context">Action上下文</param>
        /// <param name="parameter">参数上下文</param>
        public abstract void BeforeRequest(ApiActionContext context, ApiParameterDescriptor parameter);

        /// <summary>
        /// 替换带有花括号的参数的值
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="parameter">参数</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>替换成功则返回true</returns>
        protected Uri UsePathQuery(Uri uri, ApiParameterDescriptor parameter, Encoding encoding)
        {
            var replaced = false;

            // 替换Url中的参数
            if (uri.OriginalString.IndexOf('{') > -1)
            {
                string url = uri.OriginalString;
                var regex = new Regex($"{{{parameter.Name}}}", RegexOptions.IgnoreCase);
                url = regex.Replace(url, m =>
                {
                    replaced = true;
                    return parameter.Value?.ToString().UrlEncode(encoding);
                });
                if (url != uri.OriginalString)
                {
                    uri = new Uri(url);
                }
            }

            if (!replaced)
            {
                string url = uri.OriginalString;
                string queryString = EncodeParameter(parameter, encoding);
                var concat = url.IndexOf('?') > -1 ? "&" : "?";
                var relativeUri = $"{url}{concat}{queryString}{uri.Fragment}";
                uri = new Uri(uri, relativeUri);
            }

            return uri;
        }

        /// <summary>
        /// 替换带有花括号的参数的值
        /// </summary>
        /// <param name="uri">Url</param>
        /// <param name="parameters">参数列表</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>替换成功则返回true</returns>
        protected Uri UsePathQuery(Uri uri, List<ApiParameterDescriptor> parameters, Encoding encoding)
        {
            // 替换Url中的参数
            if (uri.OriginalString.IndexOf('{') > -1)
            {
                string url = uri.OriginalString;
                parameters = parameters.Where(p =>
                {
                    var replaced = false;
                    var regex = new Regex($"{{{p.Name}}}", RegexOptions.IgnoreCase);
                    url = regex.Replace(url, m =>
                    {
                        replaced = true;
                        return p.Value?.ToString().UrlEncode(encoding);
                    });
                    return !replaced;
                }).ToList();
                if (url != uri.OriginalString)
                {
                    uri = new Uri(url);
                }
            }

            if (parameters.Count > 0)
            {
                string url = uri.OriginalString;
                string queryString = EncodeParameters(parameters, Encoding.UTF8);
                var concat = url.IndexOf('?') > -1 ? "&" : "?";
                var relativeUri = $"{url}{concat}{queryString}{uri.Fragment}";
                uri = new Uri(uri, relativeUri);
            }

            return uri;
        }

        private string EncodeParameters(IEnumerable<ApiParameterDescriptor> parameters, Encoding encoding) =>
            string.Join("&", parameters.Select(parameter => EncodeParameter(parameter, encoding)).ToArray());

        private string EncodeParameter(ApiParameterDescriptor parameter, Encoding encoding) =>
            $"{parameter.Name.UrlEncode(encoding)}={ToStringUnNull(parameter.Value).UrlEncode(encoding)}";

        private string ToStringUnNull(object obj) => obj == null ? string.Empty : obj.ToString();
    }
}
