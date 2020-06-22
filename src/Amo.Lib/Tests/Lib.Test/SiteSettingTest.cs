using Amo.Lib.Test.Data;
using Moq;
using Xunit;

namespace Amo.Lib.Test
{
    public class SiteSettingTest
    {
        private readonly Mock<IReadConfig> mockReadConfig;

        // private readonly Mock<SiteSetting> mockSetting;
        public SiteSettingTest()
        {
            mockReadConfig = new Mock<IReadConfig>();
            SiteSettingFac<SiteSetting>.UpdateOrAddReadConfig(mockReadConfig.Object);

            mockReadConfig.Setup(q => q.GetValue("Test1")).Returns("32");
            mockReadConfig.Setup(q => q.GetValue("Test2")).Returns("true");
            mockReadConfig.Setup(q => q.GetValue("Test3")).Returns("Test3");
        }

        [Fact]
        public void GetSettingTest()
        {
            // Mock<SiteSetting> mockHPNSetting = new Mock<SiteSetting>();
            // SiteSettingFac<SiteSetting>.SetSetting(Sites.HPN, mockSetting.Object);
            SiteSetting setting = SiteSetting.GetSetting(Sites.HPN);
            Assert.Equal(Sites.HPN, setting.Site);
            Assert.True(setting.TestSites);
            Assert.Equal(32, setting.TestConfig1);
            Assert.True(setting.TestConfig2);
            Assert.Equal("Test3", setting.TestConfig3);
            Assert.Equal(22, setting.TestDictonary);

            SiteSetting setting2 = SiteSetting.GetSetting(Sites.TPD);
            Assert.False(setting2.TestSites);
            Assert.Equal(0, setting2.TestDictonary);
        }
    }
}
