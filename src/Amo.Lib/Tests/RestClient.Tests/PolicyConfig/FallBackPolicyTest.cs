using Amo.Lib.RestClient.PolicyConfig;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Amo.Lib.RestClient.Tests.PolicyConfig
{
    public class FallBackPolicyTest
    {
        [Fact]
        public void BaseTest1()
        {
            FallBackPolicy policy = new FallBackPolicy();
            Assert.False(policy.IsValid());
            Assert.Null(policy.Get());
            Assert.Null(policy.GetAsync());
        }

        [Fact]
        public void BaseTest2()
        {
            FallBackPolicy policy = new FallBackPolicy(action: () => { });
            Assert.True(policy.IsValid());
            Assert.NotNull(policy.Get());
            Assert.Null(policy.GetAsync());

            Assert.NotNull(policy.Action);
            Assert.Null(policy.Func);
        }

        [Fact]
        public void BaseTest3()
        {
            FallBackPolicy policy = new FallBackPolicy(func: (ct) => null);
            Assert.True(policy.IsValid());
            Assert.Null(policy.Get());
            Assert.NotNull(policy.GetAsync());

            Assert.Null(policy.Action);
            Assert.NotNull(policy.Func);
        }
    }
}
