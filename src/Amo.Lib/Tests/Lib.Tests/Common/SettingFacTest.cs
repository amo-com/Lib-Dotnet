using Amo.Lib.Tests.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Amo.Lib.Tests.Common
{
    public class SettingFacTest
    {
        private readonly IReadConfig readConfig;

        public SettingFacTest()
        {
            readConfig = new ReadConfig();
        }

        [Fact]
        public void GetSettingTest()
        {
            DataSetting.UpdateOrAddReadConfig(readConfig);

            DataSetting setting = DataSetting.GetSetting(Sites.APW);
            Assert.Equal("Test", setting.EnvironmetName);
            Assert.Equal(Sites.APW, setting.Site);
            Assert.True(setting.EnableShowApiSwagger);
            Assert.NotNull(setting.Version);
            Assert.Equal("20191228", setting.Version.Version1);
            Assert.Equal("2.0.0.1", setting.Version.Version2);
        }

        public class ReadConfig : IReadConfig
        {
            private readonly IConfiguration configuration;

            public ReadConfig()
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("./.Resources/config.json", optional: false, reloadOnChange: false)
                    .Build();
            }

            public T Get<T>(string path)
            {
                return configuration.GetSection(path).Get<T>();
            }

            public object Get(string path, Type type)
            {
                return configuration.GetSection(path).Get(type);
            }
        }
    }
}
