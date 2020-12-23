using Polly;
using System;

namespace Amo.Lib.PolicyConfig
{
    /// <summary>
    /// 重试(Retry)Api Request重试策略
    /// 1.TimeSpans
    /// 2.RetryCount,SleepDuration
    /// </summary>
    public class RetryPolicy : IPolicyConfig
    {
        public RetryPolicy()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy"/> class.
        /// 构造
        /// </summary>
        /// <param name="retryCount">重试次数</param>
        /// <param name="sleepDuration">时间间隔(ms)</param>
        public RetryPolicy(int retryCount, int sleepDuration = 0)
        {
            this.RetryCount = retryCount;
            this.SleepDuration = sleepDuration;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicy"/> class.
        /// 构造
        /// </summary>
        /// <param name="timeSpans">时间间隔(长度即为重试次数)</param>
        public RetryPolicy(params TimeSpan[] timeSpans)
        {
            this.TimeSpans = timeSpans;
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// 时间间隔(ms)
        /// </summary>
        public int SleepDuration { get; set; }

        /// <summary>
        /// 时间间隔(长度即为重试次数)
        /// </summary>
        public TimeSpan[] TimeSpans { get; set; }

        public int Index => 3;

        public Policy GetSync()
        {
            Policy policyHandle = null;

            if (!IsValid())
            {
                return policyHandle;
            }

            if (TimeSpans != null && TimeSpans.Length > 0)
            {
                policyHandle = Policy.Handle<Exception>().WaitAndRetry(TimeSpans);
            }
            else if (RetryCount > 0 && SleepDuration > 0)
            {
                policyHandle = Policy.Handle<Exception>().WaitAndRetry(RetryCount, attempt => TimeSpan.FromMilliseconds(SleepDuration));
            }
            else if (RetryCount > 0)
            {
                policyHandle = Policy.Handle<Exception>().Retry(RetryCount);
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

            if (TimeSpans != null && TimeSpans.Length > 0)
            {
                policyHandle = Policy.Handle<Exception>().WaitAndRetryAsync(TimeSpans);
            }
            else if (RetryCount > 0 && SleepDuration > 0)
            {
                policyHandle = Policy.Handle<Exception>().WaitAndRetryAsync(RetryCount, attempt => TimeSpan.FromMilliseconds(SleepDuration));
            }
            else if (RetryCount > 0)
            {
                policyHandle = Policy.Handle<Exception>().RetryAsync(RetryCount);
            }

            return policyHandle;
        }

        public bool IsValid()
        {
            return RetryCount > 0 || (TimeSpans != null && TimeSpans.Length > 0);
        }
    }
}
