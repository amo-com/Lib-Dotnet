using Amo.Lib.RestClient.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.PolicyConfig
{
    public class BreakerPolicyTest
    {
        [Fact]
        public void BaseTest1()
        {
            BreakerPolicy policy = new BreakerPolicy();
            Assert.False(policy.IsValid());
            Assert.Null(policy.Get());
            Assert.Null(policy.GetAsync());
        }

        [Fact]
        public void BaseTest2()
        {
            BreakerPolicy policy = new BreakerPolicy(6, 0);
            Assert.False(policy.IsValid());
            Assert.Null(policy.Get());
            Assert.Null(policy.GetAsync());

            Assert.Equal(6, policy.Count);
        }

        [Fact]
        public void BaseTest3()
        {
            BreakerPolicy policy = new BreakerPolicy(6, 5000);
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.NotNull(policy.GetAsync());
            Assert.Equal(6, policy.Count);
            Assert.Equal(5000, policy.Duration);
        }
    }
}
