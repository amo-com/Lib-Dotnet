using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amo.Lib.RestClient.Extensions
{
    /// <summary>
    /// Attribute扩展
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// 从方法和声明的类型中查找所有特性
        /// </summary>
        /// <typeparam name="TAttribute">查询的Attribute类型</typeparam>
        /// <param name="method">方法</param>
        /// <param name="inherit">是否穿透</param>
        /// <returns>TAttribute</returns>
        public static IEnumerable<TAttribute> FindDeclaringAttributes<TAttribute>(this MethodInfo method, bool inherit)
            where TAttribute : class
        {
            var methodAttributes = method.GetAttributes<TAttribute>(inherit);
            var interfaceAttributes = method.DeclaringType.GetTypeInfo().GetAttributes<TAttribute>(inherit);
            return methodAttributes.Concat(interfaceAttributes);
        }

        /// <summary>
        /// 获取成员的特性
        /// </summary>
        /// <typeparam name="TAttribute">查询的Attribute类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否穿透</param>
        /// <returns>TAttribute</returns>
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
        /// <typeparam name="TAttribute">查询的Attribute类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否穿透</param>
        /// <returns>TAttribute</returns>
        private static IEnumerable<TAttribute> GetAttributes<TAttribute>(this MemberInfo member, bool inherit)
            where TAttribute : class
        {
            return member
                .GetCustomAttributes(inherit)
                .OfType<TAttribute>();
        }
    }
}
