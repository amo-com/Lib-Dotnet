using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.PollyPolicyAttributes
{
    public class PollyRetryAttributeTest
    {
        [Fact]
        public void InitTest()
        {
            PollyRetryAttribute attribute = new PollyRetryAttribute(3, 35);
            Assert.Equal(3, attribute.RetryCount);
            Assert.Equal(35, attribute.SleepDuration);
            Assert.Null(attribute.TimeSpans);

            var policyConfig = attribute.CreatePolicy();
            Assert.True(policyConfig is RetryPolicy);

            var retryPolicy = policyConfig as RetryPolicy;
            Assert.Equal(3, retryPolicy.RetryCount);
            Assert.Equal(35, retryPolicy.SleepDuration);

            PollyRetryAttribute attribute2 = new PollyRetryAttribute(new TimeSpan[] { });
            Assert.NotNull(attribute2.TimeSpans);
        }

        [Fact]
        public void AttributeTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IPollyPolicyTestApi).GetMethod("RetryTest1"));
            Assert.Equal(1, descriptor.PolicyAttributes.Count);

            var policyAttribute = descriptor.PolicyAttributes[0];
            Assert.True(policyAttribute is PollyRetryAttribute);
            var retryAttribute = policyAttribute as PollyRetryAttribute;
            Assert.Equal(3, retryAttribute.RetryCount);
            Assert.Equal(30, retryAttribute.SleepDuration);
        }
    }
}
