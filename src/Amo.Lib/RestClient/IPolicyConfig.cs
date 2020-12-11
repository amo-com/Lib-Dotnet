namespace Amo.Lib.RestClient
{
    public interface IPolicyConfig
    {
        /// <summary>
        /// 层次,小的是内层,先执行
        /// 1:fallback, 2:cache, 3:retry, 4:breaker, 5:timeout, 6:bulkhead
        /// </summary>
        int Index { get; }

        /// <summary>
        /// 判定是否为有效的配置
        /// </summary>
        /// <returns>判定结果</returns>
        bool IsValid();

        /// <summary>
        /// 生成异步的PolicyHandler
        /// </summary>
        /// <returns>IAsyncPolicy</returns>
        Polly.IAsyncPolicy GetAsync();

        /// <summary>
        /// 生成PolicyPolicyHandler
        /// </summary>
        /// <returns>Policy</returns>
        Polly.Policy Get();
    }
}
