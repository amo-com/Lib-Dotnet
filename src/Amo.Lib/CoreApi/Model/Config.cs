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
        /// 环境变量名,需要按名字从环境变量中读取value
        /// ASPNETCORE_ENVIRONMENT=dev.demo
        /// </summary>
        public string Env { get; set; } = "local";

        /// <summary>
        /// ASPNETCORE_ENVIRONMENT_BASE=dev
        /// </summary>
        public string EnvBase { get; set; } = string.Empty;

        /// <summary>
        /// ASPNETCORE_CONFIG=appsettings
        /// </summary>
        public string Name { get; set; } = "appsettings";

        /// <summary>
        /// 指定配置文件路径,Docker时加载外部config,默认是空,当前程序的运行路径
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 需要appsettings.json
        /// </summary>
        public bool NeedDefault { get; set; } = true;
    }
}
