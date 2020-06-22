using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amo.Lib.Extensions
{
    public static class AttributeExtensions
    {
        /// <summary>
        /// 获取成员的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="type">成员</param>
        /// <param name="inherit">Inherit</param>
        /// <returns>特性</returns>
        public static TAttribute GetAttribute<TAttribute>(this Type type, bool inherit)
            where TAttribute : class
        {
            return type
                .GetCustomAttributes(inherit)
                .OfType<TAttribute>()
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取成员的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">Inherit</param>
        /// <returns>特性</returns>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo member, bool inherit)
            where TAttribute : class
        {
            return member
                .GetCustomAttributes(inherit)
                .OfType<TAttribute>()
                .FirstOrDefault();
        }

        /// <summary>
        /// 获取成员的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="type">成员</param>
        /// <param name="inherit">Inherit</param>
        /// <returns>特性列表</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this Type type, bool inherit)
            where TAttribute : class
        {
            return type
                .GetCustomAttributes(inherit)
                .OfType<TAttribute>();
        }

        /// <summary>
        /// 获取成员的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">Inherit</param>
        /// <returns>特性列表</returns>
        public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this MemberInfo member, bool inherit)
            where TAttribute : class
        {
            return member
                .GetCustomAttributes(inherit)
                .OfType<TAttribute>();
        }
    }
}
