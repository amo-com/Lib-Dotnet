using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using Amo.Lib.Tests.DataProxies;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Amo.Lib.Tests.Attributes
{
    public class PolicyAttributeTest
    {
        [Fact]
        public void Test1()
        {
            MethodInfo method = Helper.GetPrivateMethod(typeof(TestPolicy), "Test1");
            var policyAttributes = method?
                .GetAttributes<IPolicyAttribute>(true)
                .ToList();
            Assert.Equal(2, policyAttributes.Count);

            var retryAttribute = (PollyRetryAttribute)policyAttributes.Find(q => q is PollyRetryAttribute);
            Assert.NotNull(retryAttribute);
            Assert.Equal(3, retryAttribute.RetryCount);

            var timeoutAttribute = (PollyTimeoutAttribute)policyAttributes.Find(q => q is PollyTimeoutAttribute);
            Assert.NotNull(timeoutAttribute);
            Assert.Equal(100, timeoutAttribute.Seconds);
        }

        [Fact]
        public void Test2()
        {
            MethodInfo method = Helper.GetPrivateMethod(typeof(TestPolicy), "Test2");
            var policyAttributes = method?
                .GetAttributes<IPolicyAttribute>(true)
                .ToList();
            Assert.Single(policyAttributes);

            var retryAttribute = (PollyRetryAttribute)policyAttributes.First();
            Assert.NotNull(retryAttribute);
            Assert.Equal(3, retryAttribute.RetryCount);
            Assert.Equal(500, retryAttribute.SleepDuration);
        }

        [Fact]
        public void Test3()
        {
            MethodInfo method = Helper.GetPrivateMethod(typeof(TestPolicy), "Test3");
            var policyAttributes = method?
                .GetAttributes<IPolicyAttribute>(true)
                .ToList();
            Assert.Single(policyAttributes);

            var retryAttribute = (PollyRetryAttribute)policyAttributes.First();
            Assert.NotNull(retryAttribute);
            Assert.Equal(2, retryAttribute.TimeSpans.Length);
            Assert.Equal(100, retryAttribute.TimeSpans[0]);
        }

        [Fact]
        public void Test4()
        {
            MethodInfo method = Helper.GetPrivateMethod(typeof(TestPolicy), "Test4");
            var policyAttributes = method?
                .GetAttributes<IPolicyAttribute>(true)
                .ToList();

            var timeoutAttribute = (PollyTimeoutAttribute)policyAttributes.Find(q => q is PollyTimeoutAttribute);
            Assert.NotNull(timeoutAttribute);
            Assert.Equal(300, timeoutAttribute.Milliseconds);

            var breakerAttribute = (PollyBreakerAttribute)policyAttributes.Find(q => q is PollyBreakerAttribute);
            Assert.NotNull(breakerAttribute);
            Assert.Equal(2, breakerAttribute.Count);
            Assert.Equal(100, breakerAttribute.Duration);
        }
    }
}
