using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.RestClient.Extensions
{
    public class StringExtensions
    {
        private const char SplitChar = '/';
        private const string SplitStr = "/";

        /// <summary>
        /// 拼接Uri信息
        /// </summary>
        /// <param name="host">Host</param>
        /// <param name="routePrefix">路由前缀</param>
        /// <param name="route">路由</param>
        /// <returns>Uri</returns>
        public static Uri GetUri(string host, string routePrefix, string route)
        {
            string url = host ?? throw new Exception("未配置Host");
            url = CombineRoute(url, routePrefix);
            url = CombineRoute(url, route);

            return new Uri(url);
        }

        public static string CombineRoute(string left, string right)
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
