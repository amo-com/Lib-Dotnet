using Amo.Lib.RestClient.PolicyConfig;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests
{
    public class IPolicyConfigTest
    {
        [Fact]
        public void BaseTest1()
        {
            IPolicyConfig retryPolicy = new RetryPolicy();
            Assert.Equal(3, retryPolicy.Index);

            IPolicyConfig breakerPolicy = new BreakerPolicy();
            Assert.Equal(new BreakerPolicy().Index, breakerPolicy.Index);

            IPolicyConfig fallBackPolicy = new FallBackPolicy();
            Assert.Equal(new FallBackPolicy().Index, fallBackPolicy.Index);

            IPolicyConfig timeoutPolicy = new TimeoutPolicy();
            Assert.Equal(new TimeoutPolicy().Index, timeoutPolicy.Index);
        }
    }
}
