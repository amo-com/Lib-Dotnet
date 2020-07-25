using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.CoreApi.Common
{
    public class ConfigManager
    {
        protected static IConfiguration config;
        public static Model.VersionVo GetVersion()
        {
            if (config == null)
            {
                config = new ConfigurationBuilder().AddJsonFile("version.json", false, false).Build();
            }

            Model.VersionVo dto = new Model.VersionVo
            {
                Version1 = config.GetValue<string>("Version1"),
                Version2 = config.GetValue<string>("Version2")
            };

            return dto;
        }
    }
}
