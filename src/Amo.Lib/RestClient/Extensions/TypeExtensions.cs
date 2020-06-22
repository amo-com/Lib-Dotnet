using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Amo.Lib.RestClient.Extensions
{
    /// <summary>
    /// Type扩展
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 类型是否AllowMultiple的缓存
        /// </summary>
        private static readonly ConcurrentDictionary<Type, bool> TypeAllowMultipleCache = new ConcurrentDictionary<Type, bool>();

        /// <summary>
        /// 关联的AttributeUsageAttribute是否AllowMultiple
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>是否AllowMultiple</returns>
        public static bool IsAllowMultiple(this Type type)
        {
            return TypeAllowMultipleCache.GetOrAdd(type, t => t.IsInheritFrom<Attribute>() && t.GetCustomAttribute<AttributeUsageAttribute>(true).AllowMultiple);
        }

        /// <summary>
        /// 是否可以从TBase类型派生
        /// </summary>
        /// <typeparam name="TBase">基础类</typeparam>
        /// <param name="type">类型</param>
        /// <returns>是否可以派生</returns>
        public static bool IsInheritFrom<TBase>(this Type type)
        {
            return typeof(TBase).IsAssignableFrom(type);
        }

        /// <summary>
        /// 转换为只读列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">当前类型</param>
        /// <returns>只读类型列表</returns>
        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToList().AsReadOnly();
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>编码后的字符串</returns>
        public static string UrlEncode(this string input, Encoding encoding)
        {
            var encoded = HttpUtility.UrlEncode(input, encoding);
            return encoded?.Replace("+", "%20");
        }
    }
}
