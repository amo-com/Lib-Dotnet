namespace Amo.Lib.Model
{
    /// <summary>
    /// Config加载配置
    /// 指定环境变量名是为了命令行传参,启动时传入参数,覆盖配置
    /// </summary>
    public class ConfigEnvironment
    {
        public string EnvName { get => "ASPNETCORE_ENVIRONMENT"; }

        /// <summary>
        /// 传递ConfigPath的环境变量名ASPNETCORE_CONFIGPATH
        /// </summary>
        public string EnvConfigPath { get => "ASPNETCORE_PATH"; }

        /// <summary>
        /// 配置路径,Docker时加载外部config,默认是空,当前程序的运行路径,docker下建议/app/config
        /// </summary>
        public string ConfigPath { get; set; } = string.Empty;

        /// <summary>
        /// 传递ConfigNames的环境变量名ASPNETCORE_CONFIGPATH
        /// </summary>
        public string EnvConfigNames { get => "ASPNETCORE_CONFIGNAMES"; }

        /// <summary>
        /// 配置列表,多个用逗号隔开,示例:appsettings.json,appsettings.dev.json
        /// </summary>
        public string ConfigNames { get; set; } = "appsettings.json";
    }
}
