using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.CoreApi.Model
{
    /// <summary>
    /// /root/..../appsettings.dev.json
    /// {Env.Get(PathEnv)}{Name}.{Env.Get(EnvNameEnv)}.Type
    /// </summary>
    public class Config
    {
        /// <summary>
        /// (EnvNameEnv,PathEnv)环境变量名,需要按名字从环境变量中读取value
        /// </summary>
        public string EnvNameEnv { get; set; } = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// 指定配置文件路径,Docker时加载外部config,默认是空,当前程序的运行路径
        /// </summary>
        public string PathEnv { get; set; } = string.Empty;
        public string Name { get; set; } = "appsettings";
        public string Type { get; set; } = "json";
    }
}
