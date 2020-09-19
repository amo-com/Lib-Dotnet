using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using System;
using System.Net.Http;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.QueryParameterAttributes
{
    public class QueryAttributeTest
    {
        [Fact]
        public void BeforeRequestTest()
        {
            var method = typeof(IAubTestApi).GetMethod("PostAsync");
            var context = new TestActionContext(
                httpApiConfig: new HttpApiConfig(),
                apiActionDescriptor: new ApiActionDescriptor("http://api.dev/", method, HttpMethod.Post)
                {
                    Url = new Uri("http://api.dev/api/test/order"),
                    Method = HttpMethod.Post
                },
                null);

            var attr = new QueryAttribute();

            var p1 = new ApiParameterDescriptor(name: "orderid", value: "21", attributes: null);
            attr.BeforeRequest(context, p1);
            Assert.True(context.RequestMessage.RequestUri == new Uri("http://api.dev/api/test/order?orderid=21"));

            var p2 = new ApiParameterDescriptor(name: "date", value: "20191201", attributes: null);
            attr.BeforeRequest(context, p2);
            Assert.True(context.RequestMessage.RequestUri == new Uri("http://api.dev/api/test/order?orderid=21&date=20191201"));
        }
    }
}
