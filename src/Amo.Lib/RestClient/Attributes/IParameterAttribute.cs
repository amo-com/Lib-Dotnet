using Amo.Lib.RestClient.Contexts;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 参数标记
    /// </summary>
    public interface IParameterAttribute
    {
        /// <summary>
        /// 初始化特性
        /// </summary>
        /// <param name="context">Action上下文</param>
        /// <param name="parameter">参数上下文</param>
        void BeforeRequest(ApiActionContext context, ApiParameterDescriptor parameter);
    }
}
