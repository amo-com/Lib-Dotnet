using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.PolicyConfig;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.PollyPolicyAttributes
{
#pragma warning disable CS0618 // 类型或成员已过时
    public class PollyFallBackAttributeTest
    {
        [Fact]
        public void InitTest()
        {
            PollyFallBackAttribute attribute = new PollyFallBackAttribute(() => { });
            Assert.NotNull(attribute.Action);
            Assert.Null(attribute.Func);

            var policyConfig = attribute.CreatePolicy();
            Assert.True(policyConfig is FallBackPolicy);

            var fallBackPolicy = policyConfig as FallBackPolicy;
            Assert.NotNull(fallBackPolicy.Action);
            Assert.Null(fallBackPolicy.Func);
        }
    }
#pragma warning restore CS0618 // 类型或成员已过时
}
