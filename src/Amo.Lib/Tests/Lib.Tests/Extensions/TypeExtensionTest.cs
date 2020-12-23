using Amo.Lib.Enums;
using Amo.Lib.Extensions;
using Amo.Lib.Tests.DataProxies;
using Xunit;

namespace Amo.Lib.Tests.Extensions
{
    public class TypeExtensionTest
    {
        [Fact]
        public void ToJsonTest()
        {
            string msg = "message test";
            var s1 = msg.ToJson();
            Assert.Equal($"\"{msg}\"", s1);

            int id = 16;
            var s2 = id.ToJson();
            Assert.Equal(id.ToString(), s2);

            SiteSetting setting = new SiteSetting();
            var s3 = setting.ToJson();
            Assert.NotNull(s3);
        }

        [Fact]
        public void IsBelongTest()
        {
            Assert.True("HPN".IsBelong("HPN"));
            Assert.True("HPN".IsBelong("HPN", "APW"));
            Assert.False("KPN".IsBelong());
            Assert.False("KPN".IsBelong("HPN", "APW"));
            Assert.True(6.IsBelong(1, 6, 13));
        }
    }
}
