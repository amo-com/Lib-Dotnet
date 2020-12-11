using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.PolicyConfig;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.PollyPolicyAttributes
{
    public class PollyBreakerAttributeTest
    {
        [Fact]
        public void InitTest()
        {
            PollyBreakerAttribute attribute = new PollyBreakerAttribute(3, 3000);
            Assert.Equal(3, attribute.Count);
            Assert.Equal(3000, attribute.Duration);

            var policyConfig = attribute.CreatePolicy();
            Assert.True(policyConfig is BreakerPolicy);

            var brakerPolicy = policyConfig as BreakerPolicy;
            Assert.Equal(3, brakerPolicy.Count);
            Assert.Equal(3000, brakerPolicy.Duration);
        }

        [Fact]
        public void AttributeTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IPollyPolicyTestApi).GetMethod("BreakerTest1"));
            Assert.Equal(1, descriptor.PolicyAttributes.Count);

            var policyAttribute = descriptor.PolicyAttributes[0];
            Assert.True(policyAttribute is PollyBreakerAttribute);
            var breakerAttribute = policyAttribute as PollyBreakerAttribute;
            Assert.Equal(4, breakerAttribute.Count);
            Assert.Equal(5000, breakerAttribute.Duration);
        }
    }
}
