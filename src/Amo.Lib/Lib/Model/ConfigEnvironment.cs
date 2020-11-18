namespace Amo.Lib.Model
{
    /// <summary>
    /// Config加载配置
    /// 指定环境变量名是为了命令行传参,启动时传入参数,覆盖配置
    /// </summary>
    public class ConfigEnvironment
    {
        /// <summary>
        /// 需要appsettings.json
        /// default: false
        /// </summary>
        public bool NeedDefault { get; set; } = false;

        /// <summary>
        /// Config文件类型
        /// default:json
        /// </summary>
        public string ConfigType { get; set; } = "json";

        /// <summary>
        /// 用于指定配置环境名的环境变量名,Value会覆盖EnvName
        /// default:ASPNETCORE_ENVIRONMENT
        /// 示例:   $(Env)=$(EnvName)
        ///         ASPNETCORE_ENVIRONMENT=local
        /// </summary>
        public string Env { get; set; } = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// 配置环境名
        /// default:
        /// </summary>
        public string EnvName { get; set; } = string.Empty;

        /// <summary>
        /// 用于指定基配置环境名的环境变量名,Value会覆盖EnvBaseName
        /// default:ASPNETCORE_ENVIRONMENT_BASE
        /// 示例:   $(EnvBase)=$(EnvBaseName)
        ///         ASPNETCORE_ENVIRONMENT=local
        /// </summary>
        public string EnvBase { get; set; } = "ASPNETCORE_ENVIRONMENT_BASE";

        /// <summary>
        /// 基配置环境名
        /// default:
        /// </summary>
        public string EnvBaseName { get; set; } = string.Empty;

        /// <summary>
        /// 用于指定config配置文件的名字的环境变量名,Value会覆盖ConfigName
        /// default:ASPNETCORE_CONFIG
        /// 示例:   $(Config)=$(ConfigName)
        ///         ASPNETCORE_CONFIG=appsettings
        /// </summary>
        public string Config { get; set; } = "ASPNETCORE_CONFIG";

        /// <summary>
        /// config配置文件的名字
        /// default:appsettings
        /// </summary>
        public string ConfigName { get; set; } = "appsettings";

        /// <summary>
        /// 用于指定config配置路径的环境变量名,Value会覆盖PathName
        /// default: ASPNETCORE_PATH
        /// 示例:   $(Path)=$(PathName)
        ///         ASPNETCORE_PATH=/root/app
        /// </summary>
        public string Path { get; set; } = "ASPNETCORE_PATH";

        /// <summary>
        /// 配置路径,Docker时加载外部config,默认是空,当前程序的运行路径
        /// default:
        /// </summary>
        public string PathName { get; set; } = string.Empty;

        /// <summary>
        /// 获取ConfigFilePath
        /// </summary>
        /// <param name="type">1:default, 2:envbase, 3:env</param>
        /// <returns>ConfigFilePath</returns>
        public string GetConfigFile(int type)
        {
            string configFile = string.Empty;
            string envStr = string.Empty;
            bool needFile = false;

            switch (type)
            {
                case 1:
                    if (this.NeedDefault)
                    {
                        needFile = true;
                    }

                    break;
                case 2:
                    if (!string.IsNullOrEmpty(this.EnvBaseName) && this.EnvBaseName != this.EnvName)
                    {
                        needFile = true;
                        envStr = $".{this.EnvBaseName}";
                    }

                    break;
                case 3:
                    needFile = true;
                    envStr = string.IsNullOrEmpty(this.EnvName) ? string.Empty : $".{this.EnvName}";
                    break;
                default:
                    break;
            }

            this.PathName = string.IsNullOrEmpty(this.PathName) ? "." : this.PathName.TrimEnd('/').TrimEnd('\\');

            if (needFile)
            {
                configFile = $"{this.PathName}/{this.ConfigName}{envStr}.{this.ConfigType}";
            }

            return configFile;
        }
    }
}
