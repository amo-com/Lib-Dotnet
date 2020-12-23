using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 自动注解
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class AutowiredAttribute : Attribute
    {
        public AutowiredAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutowiredAttribute"/> class.
        /// 注解标记
        /// </summary>
        /// <param name="scope">作用域</param>
        public AutowiredAttribute(ScopeType scope)
        {
            this.ScopeType = scope;
        }

        public ScopeType ScopeType { get; private set; }
    }
}
