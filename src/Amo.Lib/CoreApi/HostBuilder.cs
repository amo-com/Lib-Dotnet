using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using System;

namespace Amo.Lib.CoreApi
{
    public static class HostBuilder
    {
        public static (IConfiguration configuration, string env) LoadConfig(string[] args)
        {
            var env = Environment.GetEnvironmentVariable(ApiCommon.ENVName);
            Console.WriteLine(env);
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false, false)
                .AddJsonFile($"appsettings.{env}.json", false, false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return (configuration, env);
        }

        /// <summary>
        /// 构建HostBuilder
        /// Startup需要自己创建一个,Common中的无法扫描Control,注册不到路由
        /// </summary>
        /// <param name="args">启动参数(启动时命令行输入)</param>
        /// <param name="configuration">Config</param>
        /// <returns>Builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration)
        {
            NLogBuilder.ConfigureNLog("nlog.config");

            IHostBuilder builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddConfiguration(configuration);
                    /*
                    config
                    .AddJsonFile($"appsettings.json", false, false)
                    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", false, false)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
                    */
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseUrls(configuration.GetValue<string>("Setting:HostUrls"))

                    // .UseStartup<Startup>()
                    .UseNLog();
                });

            return builder;
        }
    }
}
