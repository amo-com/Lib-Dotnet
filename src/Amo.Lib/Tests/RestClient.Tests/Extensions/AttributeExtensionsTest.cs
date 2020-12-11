using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Extensions;
using System.Linq;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Extensions
{
    public class AttributeExtensionsTest
    {
        [Fact]
        public void FindDeclaringAttributesTest()
        {
            var method = typeof(IPollyPolicyTestApi).GetMethod("Test4");
            var policyAttributes = method.FindDeclaringAttributes<IPolicyAttribute>(true).ToList();
            Assert.Equal(3, policyAttributes.Count);

            var method2 = typeof(PollyPolicyTestApi).GetMethod("Test4");
            var policyAttributes2 = method2.FindDeclaringAttributes<IPolicyAttribute>(true).ToList();
            Assert.Single(policyAttributes2);

            var method3 = typeof(PollyPolicyTestApi).GetMethod("RetryTest1");
            var policyAttributes3 = method3.FindDeclaringAttributes<IPolicyAttribute>(true).ToList();
            Assert.Equal(2, policyAttributes3.Count);
        }

        [Fact]
        public void GetAttributeTest()
        {
            var method = typeof(IPollyPolicyTestApi).GetMethod("Test4");
            var policyAttribute = method.GetAttribute<IPolicyAttribute>(true);
            Assert.NotNull(policyAttribute);
        }
    }
}
