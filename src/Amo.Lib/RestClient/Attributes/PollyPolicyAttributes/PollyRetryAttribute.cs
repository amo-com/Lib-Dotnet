using System;

namespace Amo.Lib.RestClient.Attributes
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
        /// Initializes a new instance of the <see cref="PollyRetryAttribute"/> class.
        /// 构造
        /// </summary>
        /// <param name="timeSpans">时间间隔(长度即为重试次数)</param>
        public PollyRetryAttribute(params TimeSpan[] timeSpans)
        {
            this.TimeSpans = timeSpans;
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
        /// 时间间隔(长度即为重试次数)
        /// </summary>
        public TimeSpan[] TimeSpans { get; private set; }

        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.RetryPolicy()
            {
                RetryCount = RetryCount,
                SleepDuration = SleepDuration,
                TimeSpans = TimeSpans
            };
        }
    }
}
