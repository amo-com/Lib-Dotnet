using Amo.Lib.RestClient.Extensions;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Fact]
        public void GetUriTest()
        {
            Assert.Throws<Exception>(() => StringExtensions.GetUri(null, "api", "part"));
            Assert.Equal(new Uri("http://dev.api/api/part"), StringExtensions.GetUri("http://dev.api", "api", "part"));
        }

        [Fact]
        public void CombineRouteTest()
        {
            Assert.Equal("api/part", StringExtensions.CombineRoute("api/", "part"));
            Assert.Equal("api/part", StringExtensions.CombineRoute("api", "/part"));
            Assert.Equal("api/part", StringExtensions.CombineRoute("api", "part"));
            Assert.Equal("api/", StringExtensions.CombineRoute("api/", null));
            Assert.Equal("part", StringExtensions.CombineRoute(null, "part"));
        }
    }
}
