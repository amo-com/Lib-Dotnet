using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;

namespace Amo.Lib.CoreApi
{
    public static class HostBuilder
    {
        public static IConfiguration LoadConfig(string[] args)
        {
            return ConfigManager.LoadConfig(ApiCommon.Environment, args);
        }

        /// <summary>
        /// 构建HostBuilder
        /// Startup需要自己创建一个,Common中的无法扫描Control,注册不到路由,只会扫描主程序路径下的路由,所以Startup需要主程序在定义一个
        /// 可以继承自Amo.Lib.CoreApi.Startup
        /// Setting:EnableApollo  apollo开关
        /// </summary>
        /// <typeparam name="T">派生的Startup</typeparam>
        /// <param name="args">启动参数(启动时命令行输入)</param>
        /// <param name="configuration">configuration</param>
        /// <returns>Builder</returns>
        public static IHostBuilder CreateHostBuilder<T>(string[] args, IConfiguration configuration)
            where T : Startup
        {
            NLogBuilder.ConfigureNLog("nlog.config");
            bool enableApollo = configuration.GetValue<bool>("Setting:EnableApollo");

            // var apolloConfig = configuration.GetSection("apollo").Get<ApolloOptions>();
            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // 先加载配置中心,再加载本地的,方便调试
                    if (enableApollo)
                    {
                        config.AddApollo(configuration.GetSection("apollo")).AddDefault();
                    }

                    config.AddConfiguration(configuration);
                })

                // UseStartup要在UseNLog之前,否则Docker报错(命令行正常)
                // Unable to start Kestrel
                // Cannot assign requested address
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls(configuration.GetValue<string>("Setting:HostUrls"))
                    .UseStartup<T>()
                    .UseNLog();
                });

            return builder;
        }

        public static string GetEnvOrConfig(string envName, string defaultValue)
        {
            return GetFirst(Environment.GetEnvironmentVariable(envName), defaultValue);
        }

        public static string GetFirst(params string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return string.Empty;
            }

            foreach (var arg in args)
            {
                if (!string.IsNullOrEmpty(arg))
                {
                    return arg;
                }
            }

            return string.Empty;
        }
    }
}
