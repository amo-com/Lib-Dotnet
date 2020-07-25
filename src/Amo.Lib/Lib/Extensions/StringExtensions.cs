using System;
using System.Collections.Generic;
using System.Web;

namespace Amo.Lib.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
    {
        public static string GetValueOrDefault(this object source, string defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }
            else if (string.IsNullOrEmpty(source.ToString()))
            {
                return defaultValue;
            }
            else
            {
                return source.ToString();
            }
        }

        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNotEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="source">输入字符串</param>
        /// <param name="prefix">要添加的前缀</param>
        /// <returns>合并后字符串</returns>
        public static string Prepend(this string source, string prefix)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return prefix + source;
            }

            return source;
        }

        /// <summary>
        /// 移除前后的括号()
        /// </summary>
        /// <param name="source">原始字符</param>
        /// <param name="symbol">待移除的符号</param>
        /// <returns>处理后的字符</returns>
        public static string TrimBracket(this string source, string symbol = "(")
        {
            if (source == null)
            {
                source = string.Empty;
            }

            string dest = source;

            if (dest.StartsWith(symbol) && dest.EndsWith(symbol))
            {
                dest = dest.Substring(1);
                dest = dest.Substring(0, dest.Length - 1);

                return dest.TrimBracket();
            }

            return dest;
        }

        public static string AddBracket(this string source, string symbol = "(")
        {
            return $"{symbol}source.TrimBracket(){symbol}";
        }

        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="param">需要编码的数据</param>
        /// <returns>编码后数据</returns>
        public static string ParamEncode(this string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return string.Empty;
            }

            return HttpUtility.UrlEncode(param).Replace("+", "%20");
        }

        public static string ParamDecode(this string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return string.Empty;
            }

            return HttpUtility.UrlDecode(param);
        }

        public static string FilterEncode(this string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return string.Empty;
            }

            filter = filter.TrimBracket();

            // 按照;进行划分
            string[] kvpArr = filter.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> encodeKvpArr = new List<string>();

            foreach (string kvp in kvpArr)
            {
                string[] pair = kvp.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (pair.Length == 2)
                {
                    string key = HttpUtility.UrlEncode(pair[0]);
                    string value = HttpUtility.UrlEncode(pair[1]);

                    string encodeKvp = string.Join("=", new string[] { key, value });
                    encodeKvpArr.Add(encodeKvp);
                }
                else
                {
                    encodeKvpArr.Add(HttpUtility.UrlEncode(kvp));
                }
            }

            string encodeFilter = string.Join(";", encodeKvpArr.ToArray());

            return encodeFilter;
        }

        /// <summary>
        /// 忽略大小写查询是否包含字符串
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <param name="checkString">查询字符串</param>
        /// <returns>是否包含</returns>
        public static bool HasContainsString(this string originalString, string checkString)
        {
            return originalString.IndexOf(checkString, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
