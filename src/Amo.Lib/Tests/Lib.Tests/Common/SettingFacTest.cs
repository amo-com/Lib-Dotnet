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

            public void Bind(string path, object instance)
            {
                configuration.Bind(path, instance);
            }

            public T GetValue<T>(string path)
            {
                return configuration.GetValue<T>(path);
            }

            public string GetValue(string path)
            {
                return configuration.GetValue<string>(path);
            }

            public object GetValue(string path, Type type)
            {
                return configuration.GetValue(type, path);
            }
        }
    }
}
