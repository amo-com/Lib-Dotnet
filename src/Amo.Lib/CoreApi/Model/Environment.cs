using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.CoreApi.Model
{
    public class Environment
    {
        /// <summary>
        /// (EnvNameEnv,PathEnv)环境变量名,需要按名字从环境变量中读取value
        /// ASPNETCORE_ENVIRONMENT=dev.demo
        /// </summary>
        public string Env { get; set; } = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// ASPNETCORE_ENVIRONMENT_BASE=dev
        /// </summary>
        public string EnvBase { get; set; } = "ASPNETCORE_ENVIRONMENT_BASE";

        /// <summary>
        /// ASPNETCORE_CONFIG=appsettings
        /// </summary>
        public string Config { get; set; } = "ASPNETCORE_CONFIG";

        /// <summary>
        /// ASPNETCORE_PATH=/root/app
        /// </summary>
        public string Path { get; set; } = "ASPNETCORE_PATH";
    }
}
