using Amo.Lib.PolicyConfig;
using System;

namespace Amo.Lib.Attributes
{
    /// <summary>
    /// 超时检测(Timeout)
    /// 1.Milliseconds
    /// 2.Seconds
    /// Milliseconds/Seconds二选一
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

        public int Seconds { get; set; }
        public int Milliseconds { get; set; }
        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.TimeoutPolicy() { Seconds = Seconds, TimeSpan = TimeSpan.FromMilliseconds(Milliseconds) };
        }
    }
}
