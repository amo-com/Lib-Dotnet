using Polly;
using System;

namespace Amo.Lib.RestClient.PolicyConfig
{
    /// <summary>
    /// 断路器(Circuit-breaker)
    /// </summary>
    public class BreakerPolicy : IPolicyConfig
    {
        public BreakerPolicy()
        {
        }

        public BreakerPolicy(int count, int duration)
        {
            this.Count = count;
            this.Duration = duration;
        }

        /// <summary>
        /// 允许的最大异常数,超过之后会触发断路
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 断路持续时间(ms)
        /// </summary>
        public int Duration { get; set; }

        public int Index => 4;
        public Policy Get()
        {
            Policy policyHandle = null;
            if (!IsValid())
            {
                return policyHandle;
            }

            policyHandle = Policy.Handle<Exception>().CircuitBreaker(Count, TimeSpan.FromMilliseconds(Duration));

            return policyHandle;
        }

        public IAsyncPolicy GetAsync()
        {
            IAsyncPolicy policyHandle = null;
            if (!IsValid())
            {
                return policyHandle;
            }

            policyHandle = Policy.Handle<Exception>().CircuitBreakerAsync(Count, TimeSpan.FromMilliseconds(Duration));

            return policyHandle;
        }

        public bool IsValid()
        {
            return Count > 0 && Duration > 0;
        }
    }
}
