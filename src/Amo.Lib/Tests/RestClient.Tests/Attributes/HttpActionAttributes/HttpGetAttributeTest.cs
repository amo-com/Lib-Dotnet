using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using System;
using System.Net.Http;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.HttpActionAttributes
{
    public class HttpGetAttributeTest
    {
        [Fact]
        public void InitTest()
        {
            var descriptor = new ApiActionDescriptor("http://api.dev/", typeof(IAubTestApi).GetMethod("PostAsync"));

            var attr1 = new HttpGetAttribute();
            attr1.Init(descriptor);
            Assert.True(descriptor.Method == HttpMethod.Get);
            Assert.True(descriptor.Route == null);
            Assert.True(descriptor.Url == new Uri("http://api.dev"));

            var attr2 = new HttpGetAttribute("part-list");
            attr2.Init(descriptor);
            Assert.True(descriptor.Method == HttpMethod.Get);
            Assert.True(descriptor.Route == "part-list");
            Assert.True(descriptor.Url == new Uri("http://api.dev/part-list"));
        }
    }
}
