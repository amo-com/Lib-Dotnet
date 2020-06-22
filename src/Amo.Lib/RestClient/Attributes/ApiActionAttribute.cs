using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.Extensions;
using System;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// ApiAction修饰特性抽象
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiActionAttribute : Attribute, IApiActionAttribute
    {
        /// <summary>
        /// 获取顺序排序索引
        /// </summary>
        public virtual int OrderIndex { get; }

        /// <summary>
        /// 获取本类型是否允许在接口与方法上重复
        /// </summary>
        public bool AllowMultiple
        {
            get => this.GetType().IsAllowMultiple();
        }

        /// <summary>
        /// 是否为Get,Post等类型标记
        /// </summary>
        public virtual bool IsMethod => false;

        /// <summary>
        /// 初始化,准备数据
        /// </summary>
        /// <param name="descriptor">Action配置</param>
        public abstract void Init(ApiActionDescriptor descriptor);

        /// <summary>
        /// 在拦截的context请求之前的事件,如果把参数拼到请求中
        /// </summary>
        /// <param name="context">Action上下文</param>
        public abstract void BeforeRequest(ApiActionContext context);
    }
}
