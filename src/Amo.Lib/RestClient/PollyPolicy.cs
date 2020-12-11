using Amo.Lib.RestClient.PolicyConfig;
using System.Collections.Generic;

namespace Amo.Lib.RestClient
{
    /// <summary>
    /// Polly策略配置
    /// </summary>
    public class PollyPolicy
    {
        // TODO:添加自定义Index顺序支持,允许设置自定义的策略先后顺序
        public List<IPolicyConfig> Policies = new List<IPolicyConfig>();

        public BreakerPolicy BreakerPolicy { get; set; }
        public FallBackPolicy FallBackPolicy { get; set; }
        public RetryPolicy RetryPolicy { get; set; }
        public TimeoutPolicy TimeoutPolicy { get; set; }

        /// <summary>
        /// 重置无效的Policy配置为Null
        /// 如果配置无效,当做无配置,直接不启用
        /// </summary>
        public void Init()
        {
            InitPolicy(BreakerPolicy);
            InitPolicy(FallBackPolicy);
            InitPolicy(RetryPolicy);
            InitPolicy(TimeoutPolicy);
        }

        private void InitPolicy(IPolicyConfig policy)
        {
            if (policy == null)
            {
                return;
            }

            if (!Policies.Contains(policy))
            {
                Policies.Add(policy);
            }
        }
    }
}
