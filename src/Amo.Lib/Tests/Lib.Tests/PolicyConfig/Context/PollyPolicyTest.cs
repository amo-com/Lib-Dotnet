using Amo.Lib.PolicyConfig;
using Xunit;

namespace Amo.Lib.Tests.PolicyConfig
{
    public class PollyPolicyTest
    {
        [Fact]
        public void InitTest1()
        {
            PollyPolicy policy = new PollyPolicy();
            Assert.Empty(policy.Policies);
            Assert.Null(policy.BreakerPolicy);
        }

        [Fact]
        public void InitTest2()
        {
            PollyPolicy policy = new PollyPolicy();
            policy.RetryPolicy = new RetryPolicy(3, 500);
            Assert.NotEmpty(policy.Policies);
            Assert.NotNull(policy.RetryPolicy);
            Assert.Null(policy.BreakerPolicy);
            Assert.Null(policy.TimeoutPolicy);
            Assert.Null(policy.FallBackPolicy);

            policy = new PollyPolicy();
            policy.BreakerPolicy = new BreakerPolicy();
            Assert.NotNull(policy.BreakerPolicy);

            policy = new PollyPolicy();
            policy.TimeoutPolicy = new TimeoutPolicy(100);
            Assert.NotNull(policy.TimeoutPolicy);

            policy = new PollyPolicy();
            policy.FallBackPolicy = null;
            Assert.Null(policy.FallBackPolicy);
        }
    }
}
