using Amo.Lib.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib
{
    public static class ConfigManager
    {
        /// <summary>
        /// 加载Config
        /// 加载顺序示例
        ///     .AddJsonFile("appsetting.json") // environment.NeedDefault = true
        ///     .AddJsonFile("appsetting.dev.json") // environment.EnvBaseName = dev
        ///     .AddJsonFile("appsetting.dev.apw.json") // environment.EnvName = dev.apw
        /// </summary>
        /// <param name="environment">Config依赖配置</param>
        /// <param name="args">命令行参数,可以覆盖配置文件的信息</param>
        /// <returns>Config,环境名</returns>
        public static IConfiguration LoadConfig(ConfigEnvironment environment, string[] args)
        {
            environment.EnvBaseName = GetEnvOrConfig(environment.EnvBase, environment.EnvBaseName);
            environment.EnvName = GetEnvOrConfig(environment.Env, environment.EnvName);
            environment.ConfigName = GetEnvOrConfig(environment.Config, environment.ConfigName);
            environment.PathName = GetEnvOrConfig(environment.Path, environment.PathName);

            List<string> configPaths = new List<string>();
            var configBuilder = new ConfigurationBuilder();

            // Default
            AddConfig(configBuilder, configPaths, environment.GetConfigFile(1));

            // Base
            AddConfig(configBuilder, configPaths, environment.GetConfigFile(2));

            // Env
            AddConfig(configBuilder, configPaths, environment.GetConfigFile(3));

            configBuilder.AddEnvironmentVariables().AddCommandLine(args);
            IConfiguration configuration = configBuilder.Build();

            return configuration;
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

            // return args.ToList().Find(q => !string.IsNullOrEmpty(q)) ?? string.Empty;
            foreach (var arg in args)
            {
                if (!string.IsNullOrEmpty(arg))
                {
                    return arg;
                }
            }

            return string.Empty;
        }

        private static void AddConfig(ConfigurationBuilder builder, List<string> configPaths, string newConfigPath)
        {
            if (!string.IsNullOrEmpty(newConfigPath) && !configPaths.Contains(newConfigPath))
            {
                configPaths.Add(newConfigPath);
                builder.AddJsonFile(newConfigPath);
            }
        }
    }
}
