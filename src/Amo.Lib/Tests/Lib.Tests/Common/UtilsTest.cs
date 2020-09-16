using Amo.Lib.Enums;
using Xunit;

namespace Amo.Lib.Tests.Common
{
    public class UtilsTest
    {
        [Fact]
        public void CutStrTest()
        {
            Assert.Null(Utils.CutStr("https://yq.aliyun.com/album/233.png", "_"));
            Assert.Null(Utils.CutStr("https://yq.aliyun.com/album/233.png", "_", firstOrLast: true));
            Assert.Equal("https://yq.aliyun.com/album/233.png", Utils.CutStr("https://yq.aliyun.com/album/233.png", "_", previousOrNext: true));
            Assert.Equal("https://yq.aliyun.com/album/233.png", Utils.CutStr("https://yq.aliyun.com/album/233.png", "_", true, true));

            Assert.Equal("png", Utils.CutStr("https://yq.aliyun.com/album/233.png", "."));
            Assert.Equal("aliyun.com/album/233.png", Utils.CutStr("https://yq.aliyun.com/album/233.png", ".", firstOrLast: true));
            Assert.Equal("https://yq.aliyun.com/album/233", Utils.CutStr("https://yq.aliyun.com/album/233.png", ".", previousOrNext: true));
            Assert.Equal("https://yq", Utils.CutStr("https://yq.aliyun.com/album/233.png", ".", true, true));
        }

        [Fact]
        public void GetLevelTest()
        {
            Assert.Equal(LogLevel.Warn, Utils.GetLevel((int)EventType.SiteUnValid));
            Assert.Equal(LogLevel.Error, Utils.GetLevel((int)EventType.ApiError));
            Assert.Equal(LogLevel.Error, Utils.GetLevel((int)EventType.ReturnNull));
            Assert.Equal(LogLevel.Info, Utils.GetLevel((int)EventType.Success));
        }
    }
}
