using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Attributes.QueryParameterAttributes
{
    public class HeaderAttributeTest
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

            var attr = new HeaderAttribute();

            var parameter = new ApiParameterDescriptor(name: "logkey", value: "1234567", attributes: null);
            attr.BeforeRequest(context, parameter);
            context.RequestMessage.Headers.TryGetValues("logkey", out IEnumerable<string> values);
            Assert.Equal("1234567", values.FirstOrDefault());
        }
    }
}
