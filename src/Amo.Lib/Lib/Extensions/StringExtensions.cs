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

        public static string Prepend(this string source, string prefix)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return prefix + source;
            }

            return source;
        }

        public static string RemoveQuery(this string url)
        {
            if (!url.Contains("?"))
            {
                return url;
            }
            else
            {
                int index = url.IndexOf("?");

                return url.Substring(0, index);
            }
        }

        /// <summary>
        /// 移除前后的括号()
        /// </summary>
        /// <param name="source">原始字符</param>
        /// <returns>处理后的字符</returns>
        public static string TrimBracket(this string source)
        {
            if (source == null)
            {
                source = string.Empty;
            }

            string dest = source;

            if (dest.StartsWith("(") && dest.EndsWith(")"))
            {
                dest = dest.Substring(1);
                dest = dest.Substring(0, dest.Length - 1);

                return dest.TrimBracket();
            }

            return dest;
        }

        public static string ParamEncode(this string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                return string.Empty;
            }

            return HttpUtility.UrlEncode(param);
        }
    }
}
