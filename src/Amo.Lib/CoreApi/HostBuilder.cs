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
            var config = ApiCommon.Config;
            var environment = ApiCommon.Environment;
            var envBase = GetEnvOrConfig(environment.EnvBase, config.EnvBase);
            var env = GetEnvOrConfig(environment.Env, config.Env);
            var configName = GetEnvOrConfig(environment.Config, config.Name);
            var configPath = GetEnvOrConfig(environment.Path, config.Path);

            configPath = configPath.Trim('/').Trim('\\');

            Console.WriteLine($"Env:{env}:::EnvBase:{envBase}:::Config:{configName}:::Path:{configPath}");

            var configBuilder = new ConfigurationBuilder();
            if (config.NeedDefault)
            {
                configBuilder.AddJsonFile($"{configPath}/{configName}.json", optional: true, reloadOnChange: true);
            }

            if (!string.IsNullOrEmpty(envBase) && envBase != env)
            {
                configBuilder.AddJsonFile($"{configPath}/{configName}.{envBase}.json", optional: true, reloadOnChange: true);
            }

            configBuilder.AddJsonFile($"{configPath}/{configName}.{env}.json", optional: true, reloadOnChange: true);
            configBuilder.AddEnvironmentVariables().AddCommandLine(args);
            IConfiguration configuration = configBuilder.Build();

            return (configuration, env);
        }

        /// <summary>
        /// 构建HostBuilder
        /// Startup需要自己创建一个,Common中的无法扫描Control,注册不到路由,只会扫描主程序路径下的路由,所以Startup需要主程序在定义一个
        /// 可以继承自Amo.Lib.CoreApi.Startup
        /// </summary>
        /// <typeparam name="T">派生的Startup</typeparam>
        /// <param name="args">启动参数(启动时命令行输入)</param>
        /// <param name="configuration">configuration</param>
        /// <returns>Builder</returns>
        public static IHostBuilder CreateHostBuilder<T>(string[] args, IConfiguration configuration)
            where T : Startup
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
