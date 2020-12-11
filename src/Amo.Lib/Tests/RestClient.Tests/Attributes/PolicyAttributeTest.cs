using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.PolicyConfig;
using System.Linq;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes
{
    public class PolicyAttributeTest
    {
        [Fact]
        public void AttributeTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IPollyPolicyTestApi).GetMethod("Test4"));
            Assert.Equal(3, descriptor.PolicyAttributes.Count);

            var pollyRetryAttribute = descriptor.PolicyAttributes.First(q => q is PollyRetryAttribute);
            Assert.NotNull(pollyRetryAttribute);
            var retryAttribute = pollyRetryAttribute as PollyRetryAttribute;
            Assert.Equal(3, retryAttribute.RetryCount);
            Assert.Equal(30, retryAttribute.SleepDuration);

            var pollyBreakerAttribute = descriptor.PolicyAttributes.First(q => q is PollyBreakerAttribute);
            Assert.NotNull(pollyBreakerAttribute);
            var breakerAttribute = pollyBreakerAttribute as PollyBreakerAttribute;
            Assert.Equal(4, breakerAttribute.Count);
            Assert.Equal(5000, breakerAttribute.Duration);

            var pollyTimeoutAttribute = descriptor.PolicyAttributes.First(q => q is PollyTimeoutAttribute);
            Assert.NotNull(pollyTimeoutAttribute);
            var timeoutAttribute = pollyTimeoutAttribute as PollyTimeoutAttribute;
            Assert.Equal(5, timeoutAttribute.Seconds);

            var policies = descriptor.PolicyAttributes.Select(q => q.CreatePolicy()).ToList();
            Assert.NotNull(policies.Find(q => q is RetryPolicy));
            Assert.NotNull(policies.Find(q => q is BreakerPolicy));
            Assert.NotNull(policies.Find(q => q is TimeoutPolicy));
        }
    }
}
