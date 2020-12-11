using Polly;
using System;

namespace Amo.Lib.RestClient.PolicyConfig
{
    /// <summary>
    /// 超时检测(Timeout)
    /// 1.TimeSpan
    /// 2.Seconds
    /// </summary>
    public class TimeoutPolicy : IPolicyConfig
    {
        public TimeoutPolicy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutPolicy"/> class.
        /// 超时检测
        /// </summary>
        /// <param name="seconds">等待时间</param>
        public TimeoutPolicy(int seconds)
        {
            this.Seconds = seconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutPolicy"/> class.
        /// 超时检测
        /// </summary>
        /// <param name="timeSpan">等待时间</param>
        public TimeoutPolicy(TimeSpan timeSpan)
        {
            this.TimeSpan = timeSpan;
        }

        public int Seconds { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public int Index => 5;

        public Policy Get()
        {
            Policy policyHandle = null;
            if (!IsValid())
            {
                return policyHandle;
            }

            if (TimeSpan != null && TimeSpan > TimeSpan.Zero)
            {
                policyHandle = Policy.Timeout(TimeSpan);
            }
            else if (Seconds > 0)
            {
                policyHandle = Policy.Timeout(Seconds);
            }

            return policyHandle;
        }

        public IAsyncPolicy GetAsync()
        {
            IAsyncPolicy policyHandle = null;
            if (!IsValid())
            {
                return policyHandle;
            }

            if (TimeSpan != null && TimeSpan > TimeSpan.Zero)
            {
                policyHandle = Policy.TimeoutAsync(TimeSpan);
            }
            else if (Seconds > 0)
            {
                policyHandle = Policy.TimeoutAsync(Seconds);
            }

            return policyHandle;
        }

        public bool IsValid()
        {
            return Seconds > 0 || (TimeSpan != null && TimeSpan > TimeSpan.Zero);
        }
    }
}
