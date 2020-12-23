using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 需要注册的拦截器
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class InterceptsAttribute : Attribute
    {
        public InterceptsAttribute(params Type[] types)
        {
            this.Types = types;
        }

        /// <summary>
        /// 继承自IInterceptor的拦截器,必须继承自接口IInterceptor,否则无效
        /// </summary>
        public Type[] Types { get; private set; }
    }
}
