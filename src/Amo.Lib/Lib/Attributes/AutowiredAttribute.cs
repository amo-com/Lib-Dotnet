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
        private readonly ScopeType scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutowiredAttribute"/> class.
        /// 注解标记
        /// </summary>
        /// <param name="scope">作用域</param>
        public AutowiredAttribute(ScopeType scope)
        {
            this.scope = scope;
        }

        public ScopeType ScopeType => this.scope;
    }
}
