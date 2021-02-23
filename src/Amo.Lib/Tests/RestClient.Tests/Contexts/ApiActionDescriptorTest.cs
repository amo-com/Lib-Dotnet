using Amo.Lib.RestClient.Attributes;
using Amo.Lib.RestClient.Contexts;
using System;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Contexts
{
    public class ApiActionDescriptorTest
    {
        [Fact]
        public void InitTest()
        {
            Assert.Throws<ArgumentNullException>(() => new ApiActionDescriptor(null, null));

            var method = typeof(IAubTestApi).GetMethod("PostAsync");
            var descriptor = new ApiActionDescriptor("http://api.dev/", method);
            Assert.NotNull(descriptor.Uri);
        }

        [Fact]
        public void NewTest()
        {
            var method = typeof(IAubTestApi).GetMethod("PostAsync");
            var descriptor = new ApiActionDescriptor("http://api.dev/", method);

            Assert.True(descriptor.Attributes.Count == 3);
            Assert.True(descriptor.Name == method.Name);
            Assert.True(descriptor.Host == "http://api.dev/");
            Assert.True(descriptor.Member == method);
            Assert.True(descriptor.Parameters.Count == 2);
        }
    }
}
