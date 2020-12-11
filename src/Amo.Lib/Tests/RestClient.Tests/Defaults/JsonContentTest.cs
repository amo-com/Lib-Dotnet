using Amo.Lib.RestClient.Defaults;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Amo.Lib.RestClient.Tests.Defaults
{
    public class JsonContentTest
    {
        [Fact]
        public void InitTest()
        {
            JsonContent content = new JsonContent(null, Encoding.ASCII);
            Assert.NotNull(content);

            Assert.Equal("application/json", JsonContent.MediaType);
        }
    }
}
