using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.HttpActionAttributes
{
    public class HttpRoutePrefixAttributeTest
    {
        [Fact]
        public void NewTest()
        {
            var p1 = new HttpRoutePrefixAttribute("user");
            Assert.True(p1.RoutePrefix == "user");
        }

        [Fact]
        public void InitTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IAubTestApi).GetMethod("PostAsync"));

            var attr1 = new HttpRoutePrefixAttribute("order-history");
            attr1.Init(descriptor);
            Assert.True(descriptor.RoutePrefix == "order-history");
        }
    }
}
