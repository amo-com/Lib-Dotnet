using Amo.Lib.Tests.DataProxies;
using Moq;
using Xunit;

namespace Amo.Lib.Tests
{
    public class SiteSettingTest
    {
        private readonly Mock<IReadConfig> mockReadConfig;

        // private readonly Mock<SiteSetting> mockSetting;
        public SiteSettingTest()
        {
            mockReadConfig = new Mock<IReadConfig>();
            SettingFac<SiteSetting>.UpdateOrAddReadConfig(mockReadConfig.Object);

            mockReadConfig.Setup(q => q.Get("Test1", typeof(int))).Returns(32);
            mockReadConfig.Setup(q => q.Get("Test2", typeof(bool))).Returns(true);
            mockReadConfig.Setup(q => q.Get("Test3", typeof(string))).Returns("Test3");

            mockReadConfig.Setup(q => q.Get("Switch1", typeof(string))).Returns("HPN,GPG;APW");
            mockReadConfig.Setup(q => q.Get("Switch2", typeof(string))).Returns("HPN,GPG;APW");
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
            Assert.True(setting.TestSwitch1);
            Assert.False(setting.TestSwitch2);

            SiteSetting setting2 = SiteSetting.GetSetting(Sites.TPD);
            Assert.False(setting2.TestSites);
        }

        [Fact]
        public void NullTest()
        {
            SiteSetting setting = default(SiteSetting);
            Assert.Null(setting);
        }
    }
}
