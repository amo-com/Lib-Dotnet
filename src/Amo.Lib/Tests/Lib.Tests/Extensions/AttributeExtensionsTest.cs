using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Amo.Lib.Tests.Extensions
{
    public class AttributeExtensionsTest
    {
        [PollyBreaker]
        [PollyRetry]
        internal interface ITest
        {
            [PollyRetry]
            void Method0();
        }

        [Fact]
        public void GetAttributeTest1()
        {
            var policy = typeof(ITest).GetAttribute<IPolicyAttribute>(false);
            Assert.NotNull(policy);
        }

        [Fact]
        public void GetAttributeTest2()
        {
            MethodInfo method = Helper.GetMethod(typeof(ITest), "Method0");
            var policy = method.GetAttribute<IPolicyAttribute>(false);
            Assert.NotNull(policy);
            Assert.IsType<PollyRetryAttribute>(policy);
        }

        [Fact]
        public void GetAttributesTest1()
        {
            var policies = typeof(ITest).GetAttributes<IPolicyAttribute>(false).ToList();
            Assert.NotNull(policies);
            Assert.Equal(2, policies.Count);
        }

        [Fact]
        public void GetAttributesTest2()
        {
            MethodInfo method = Helper.GetMethod(typeof(ITest), "Method0");
            var policies = method.GetAttributes<IPolicyAttribute>(false).ToList();
            Assert.NotNull(policies);
            Assert.Single(policies);
        }
    }
}
