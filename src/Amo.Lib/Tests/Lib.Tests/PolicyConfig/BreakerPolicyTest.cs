using Amo.Lib.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.Tests.PolicyConfig
{
    public class BreakerPolicyTest
    {
        [Fact]
        public void BaseTest1()
        {
            BreakerPolicy policy = new BreakerPolicy();
            Assert.False(policy.IsValid());
            Assert.Null(policy.GetSync());
            Assert.Null(policy.GetAsync());
            Assert.Equal(4, policy.Index);
        }

        [Fact]
        public void BaseTest2()
        {
            BreakerPolicy policy = new BreakerPolicy(6, 0);
            Assert.False(policy.IsValid());
            Assert.Null(policy.GetSync());
            Assert.Null(policy.GetAsync());

            Assert.Equal(6, policy.Count);
        }

        [Fact]
        public void BaseTest3()
        {
            BreakerPolicy policy = new BreakerPolicy(6, 5000);
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.GetSync());
            Assert.NotNull(policy.GetAsync());
            Assert.Equal(6, policy.Count);
            Assert.Equal(5000, policy.Duration);
        }
    }
}
