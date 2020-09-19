using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.HttpActionAttributes
{
    public class HttpHostAttributeTest
    {
        [Fact]
        public void NewTest()
        {
            var h1 = new HttpHostAttribute("http://api.test.cc/");
            Assert.True(h1.Host == "http://api.test.cc/");
        }

        [Fact]
        public void InitTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IAubTestApi).GetMethod("PostAsync"));

            var attr1 = new HttpHostAttribute("http://api.test.cc/");
            attr1.Init(descriptor);
            Assert.True(descriptor.Host == "http://api.dev/");

            descriptor.Host = null;
            var attr2 = new HttpHostAttribute("http://api.test.cc/");
            attr2.Init(descriptor);
            Assert.True(descriptor.Host == "http://api.test.cc/");
        }
    }
}
