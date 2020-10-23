using System.Collections.Generic;

namespace Amo.Lib.CoreApi
{
    /// <summary>
    /// 基础配置信息,需要在所有配置启用前赋值,HostBuild/Log/GlobalFilter等都是依赖于此
    /// </summary>
    public static class ApiCommon
    {
        /// <summary>
        /// Swagger显示的Api Document Name
        /// default: Demo
        /// </summary>
        public static string ApiName = "Demo";

        /// <summary>
        /// default: AllowCors
        /// </summary>
        public static string CorsName = "AllowCors";

        /// <summary>
        /// default: log
        /// </summary>
        public static string LogName = "log";

        /// <summary>
        /// Config配置, Config对应环境变量名字
        /// </summary>
        public static Lib.Model.ConfigEnvironment Environment = new Lib.Model.ConfigEnvironment();

        /// <summary>
        /// 需要注册DI的Scoped列表
        /// </summary>
        public static List<string> Scopeds;

        /// <summary>
        /// 需要注册DI的命名空间前缀列表,满足这个命名前缀的组件都做DI扫描,将所有打了Autowired的接口都加入DI
        /// </summary>
        public static List<string> Prefixs;

        /// <summary>
        /// 需要注册DI的命名空间列表,满足这个命名前缀的组件都做DI扫描,将所有打了Autowired的接口都加入DI
        /// </summary>
        public static List<string> NameSpaces;

        /// <summary>
        /// Swagger显示使用的组件xml文件
        /// </summary>
        public static List<string> XmlDocumentes;
    }
}
