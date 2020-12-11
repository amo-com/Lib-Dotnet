namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// 断路器(Circuit-breaker)
    /// </summary>
    public class PollyBreakerAttribute : PolicyAttribute
    {
        public PollyBreakerAttribute()
        {
        }

        public PollyBreakerAttribute(int count, int duration)
        {
            this.Count = count;
            this.Duration = duration;
        }

        public int Count { get; set; }

        /// <summary>
        /// 断路持续时间(ms)
        /// </summary>
        public int Duration { get; set; }
        public override IPolicyConfig CreatePolicy()
        {
            return new PolicyConfig.BreakerPolicy()
            {
                Count = Count,
                Duration = Duration
            };
        }
    }
}
