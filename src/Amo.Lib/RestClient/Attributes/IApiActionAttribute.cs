using Amo.Lib.RestClient.Contexts;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// Action标记
    /// </summary>
    public interface IApiActionAttribute
    {
        /// <summary>
        /// 获取顺序排序的索引
        /// </summary>
        int OrderIndex { get; }

        /// <summary>
        /// 获取本类型是否允许在接口与方法上重复
        /// </summary>
        bool AllowMultiple { get; }

        /// <summary>
        /// 是否为HttpMethod方法
        /// </summary>
        bool IsMethod { get; }

        /// <summary>
        /// 初始化特性
        /// </summary>
        /// <param name="descriptor">Action配置</param>
        void Init(ApiActionDescriptor descriptor);

        /// <summary>
        /// 请求前配置参数
        /// </summary>
        /// <param name="context">Action上下文</param>
        void BeforeRequest(ApiActionContext context);
    }
}
