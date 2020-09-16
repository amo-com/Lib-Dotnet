using Amo.Lib.Attributes;

namespace Amo.Lib.Tests.Data
{
    /// <summary>
    /// Fun
    /// </summary>
    public partial class SiteSetting
    {
        public static SiteSetting GetSetting(string site)
        {
            return SettingFac<SiteSetting>.GetSetting(site);
        }

        public static bool UpdateOrAddReadConfig(IReadConfig readConfig)
        {
            return SettingFac<SiteSetting>.UpdateOrAddReadConfig(readConfig);
        }
    }

    /// <summary>
    /// Property
    /// </summary>
    public partial class SiteSetting
    {
        [Desc(DataConst.SiteParam)]
        public virtual string Site { get; protected set; }

        [Sites(Sites.HPN, Sites.HPN, Sites.GPG)]
        public virtual bool TestSites { get; protected set; }

        [Config("Test1")]
        public virtual int TestConfig1 { get; protected set; }

        [Config("Test2")]
        public virtual bool TestConfig2 { get; protected set; }

        [Config("Test3")]
        public virtual string TestConfig3 { get; protected set; }
    }
}
