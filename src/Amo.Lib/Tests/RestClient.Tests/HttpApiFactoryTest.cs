using System;
using System.Net.Http;
using Xunit;

namespace Amo.Lib.RestClient.Tests
{
    public class HttpApiFactoryTest
    {
        [Fact]
        public void ConstructorTest()
        {
            HttpApiFactory factory = new HttpApiFactory("DEMO", typeof(IAubTestApi));
            Assert.Equal(typeof(IAubTestApi), factory.InterfaceType);
        }
    }
}
