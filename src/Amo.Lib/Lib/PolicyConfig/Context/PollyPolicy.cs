using System.Collections.Generic;

namespace Amo.Lib.PolicyConfig
{
    /// <summary>
    /// Polly策略配置
    /// </summary>
    public class PollyPolicy
    {
        // Policy列表
        public List<IPolicyConfig> Policies = new List<IPolicyConfig>();

        // Policy排序
        public List<int> SortIndexs = new List<int>();

        private BreakerPolicy breakerPolicy;
        private FallBackPolicy fallBackPolicy;
        private RetryPolicy retryPolicy;
        private TimeoutPolicy timeoutPolicy;

        public BreakerPolicy BreakerPolicy
        {
            get => breakerPolicy;
            set
            {
                breakerPolicy = value;
                InitPolicy(value);
            }
        }

        public FallBackPolicy FallBackPolicy
        {
            get => fallBackPolicy;
            set
            {
                fallBackPolicy = value;
                InitPolicy(value);
            }
        }

        public RetryPolicy RetryPolicy
        {
            get => retryPolicy;
            set
            {
                retryPolicy = value;
                InitPolicy(value);
            }
        }

        public TimeoutPolicy TimeoutPolicy
        {
            get => timeoutPolicy;
            set
            {
                timeoutPolicy = value;
                InitPolicy(value);
            }
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
