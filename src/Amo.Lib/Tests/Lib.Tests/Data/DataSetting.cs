using Amo.Lib.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Tests.Data
{
    /// <summary>
    /// 静态参数
    /// </summary>
    public partial class DataSetting
    {
        public static DataSetting GetSetting(string site)
        {
            return SettingFac<DataSetting>.GetSetting(site);
        }

        public static bool UpdateOrAddReadConfig(IReadConfig readConfig)
        {
            return SettingFac<DataSetting>.UpdateOrAddReadConfig(readConfig);
        }

        public static void GetOrUpdateSetting(string site)
        {
            SettingFac<DataSetting>.GetOrUpdateSetting(site);
        }
    }

    /// <summary>
    /// Setting配置
    /// </summary>
    public partial class DataSetting
    {
        [Desc(DataConst.SiteParam)]
        public virtual string Site { get; protected set; }

        [Config("Setting:EnvironmentName")]
        public virtual string EnvironmetName { get; protected set; }

        [Config("Setting:EnableShowApiSwagger")]
        public virtual bool EnableShowApiSwagger { get; protected set; }

        [Config("Setting:Version", true)]
        public virtual VersionDto Version { get; protected set; }
    }
}
