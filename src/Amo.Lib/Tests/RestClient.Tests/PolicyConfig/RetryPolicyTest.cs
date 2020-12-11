using Amo.Lib.RestClient.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.PolicyConfig
{
    public class RetryPolicyTest
    {
        [Fact]
        public void BaseTest1()
        {
            RetryPolicy policy = new RetryPolicy();
            Assert.False(policy.IsValid());
            Assert.Null(policy.Get());
            Assert.Null(policy.GetAsync());
        }

        [Fact]
        public void BaseTest2()
        {
            RetryPolicy policy = new RetryPolicy(6);
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.NotNull(policy.GetAsync());

            Assert.Equal(6, policy.RetryCount);
            Assert.Equal(0, policy.SleepDuration);
            Assert.Null(policy.TimeSpans);

            RetryPolicy policy1 = new RetryPolicy(0, 500);
            Assert.False(policy1.IsValid());
            Assert.Null(policy1.Get());
            Assert.Null(policy1.GetAsync());
        }

        [Fact]
        public void BaseTest5()
        {
            RetryPolicy policy = new RetryPolicy(6, 500);
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.NotNull(policy.GetAsync());

            Assert.Equal(6, policy.RetryCount);
            Assert.Equal(500, policy.SleepDuration);
            Assert.Null(policy.TimeSpans);
        }

        [Fact]
        public void BaseTest3()
        {
            RetryPolicy policy = new RetryPolicy(TimeSpan.Zero);
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.NotNull(policy.GetAsync());
        }

        [Fact]
        public void BaseTest4()
        {
            RetryPolicy policy = new RetryPolicy(TimeSpan.FromMilliseconds(2000));
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.NotNull(policy.GetAsync());

            Assert.Single(policy.TimeSpans);
        }
    }
}
