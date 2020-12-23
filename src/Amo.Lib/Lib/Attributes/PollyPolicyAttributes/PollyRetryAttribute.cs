using Amo.Lib.PolicyConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 重试(Retry)Api Request重试策略
    /// 1.TimeSpans
    /// 2.RetryCount,SleepDuration
    /// </summary>
    public class PollyRetryAttribute : PolicyAttribute
    {
        public PollyRetryAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollyRetryAttribute"/> class.
        /// 构造
        /// </summary>
        /// <param name="retryCount">重试次数</param>
        /// <param name="sleepDuration">时间间隔(ms)</param>
        public PollyRetryAttribute(int retryCount, int sleepDuration = 0)
        {
            this.RetryCount = retryCount;
            this.SleepDuration = sleepDuration;
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// 时间间隔(ms)
        /// </summary>
        public int SleepDuration { get; private set; }

        /// <summary>
        /// 时间间隔ms(长度即为重试次数)
        /// </summary>
        public int[] TimeSpans { get; set; }

        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.RetryPolicy()
            {
                RetryCount = RetryCount,
                SleepDuration = SleepDuration,
                TimeSpans = TimeSpans?.ToList().Select(q => TimeSpan.FromMilliseconds(q)).ToArray()
            };
        }
    }
}
