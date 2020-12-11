using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Amo.Lib.RestClient.Tests
{
    public class PollyPolicyTest
    {
        [Fact]
        public void InitTest()
        {
            PollyPolicy policy = new PollyPolicy();
            Assert.Empty(policy.Policies);
            Assert.Null(policy.BreakerPolicy);

            policy.Init();
            Assert.Empty(policy.Policies);
        }
    }
}
