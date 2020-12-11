using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.PollyPolicyAttributes
{
    public class PollyTimeoutAttributeTest
    {
        [Fact]
        public void InitTest()
        {
            PollyTimeoutAttribute attribute = new PollyTimeoutAttribute(5);
            Assert.Equal(5, attribute.Seconds);

            var policyConfig = attribute.CreatePolicy();
            Assert.True(policyConfig is TimeoutPolicy);

            var timeoutPolicy = policyConfig as TimeoutPolicy;
            Assert.Equal(5, timeoutPolicy.Seconds);

            PollyTimeoutAttribute attribute2 = new PollyTimeoutAttribute(TimeSpan.FromMilliseconds(50));
            Assert.Equal(TimeSpan.FromMilliseconds(50).TotalMilliseconds, attribute2.TimeSpan.TotalMilliseconds);
        }

        [Fact]
        public void AttributeTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IPollyPolicyTestApi).GetMethod("TimeoutTest1"));
            Assert.Equal(1, descriptor.PolicyAttributes.Count);

            var policyAttribute = descriptor.PolicyAttributes[0];
            Assert.True(policyAttribute is PollyTimeoutAttribute);
            var timeoutAttribute = policyAttribute as PollyTimeoutAttribute;
            Assert.Equal(8, timeoutAttribute.Seconds);
        }
    }
}
