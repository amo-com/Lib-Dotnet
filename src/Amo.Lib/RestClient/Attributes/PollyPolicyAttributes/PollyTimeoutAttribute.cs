using System;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 超时检测(Timeout)
    /// 1.TimeSpan
    /// 2.Seconds
    /// </summary>
    public class PollyTimeoutAttribute : PolicyAttribute
    {
        public PollyTimeoutAttribute()
        {
        }

        public PollyTimeoutAttribute(int seconds)
        {
            this.Seconds = seconds;
        }

        public PollyTimeoutAttribute(TimeSpan timeSpan)
        {
            this.TimeSpan = timeSpan;
        }

        public int Seconds { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.TimeoutPolicy() { Seconds = Seconds, TimeSpan = TimeSpan };
        }
    }
}
