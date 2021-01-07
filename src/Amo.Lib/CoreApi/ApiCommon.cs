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

        /// <summary>
        /// 读取配置中的信息节点
        /// </summary>
        public static class Appsetting
        {
            /// <summary>
            /// Ratelimit的Section节点,配置限流策略,这里是Section节点,内部子节点配置为RateLimiting格式
            /// </summary>
            public static string ReteLimitSection = "IpRateLimiting";

            /// <summary>
            /// Consul的Section节点,配置Consul注册中心配置
            /// </summary>
            public static string ConsulSection = "consul";

            /// <summary>
            /// Apollo的Section节点,配置Apollo配置中心的配置
            /// </summary>
            public static string ApolloSection = "apollo";

            /// <summary>
            /// Api路由前缀,在Control前加统一前缀
            /// 示例:api
            /// 效果:/api/[control]/[route]
            /// </summary>
            public static string ApiRoutePrefix = "Setting:ApiRoutePrefix";

            /// <summary>
            /// 是否开始api的swagger文档,默认关闭
            /// 访问路径:/api/index.html
            /// </summary>
            public static string EnableShowApiSwagger = "Setting:EnableShowApiSwagger";

            /// <summary>
            /// 是否开启服务注册,注册到注册中心
            /// </summary>
            public static string EnableRegister = "Setting:EnableRegister";

            /// <summary>
            /// 是否开启服务发现,到注册中心调度服务
            /// </summary>
            public static string EnableDiscover = "Setting:EnableDiscover";

            /// <summary>
            /// 是否启用Apollo配置中心
            /// </summary>
            public static string EnableApollo = "Setting:EnableApollo";

            /// <summary>
            /// 服务监听的端口
            /// </summary>
            public static string HostUrls = "Setting:HostUrls";
        }
    }
}
