using Amo.Lib.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Amo.Lib
{
    public static class ConfigManager
    {
        /// <summary>
        /// 加载Config
        /// </summary>
        /// <param name="environment">Config依赖配置</param>
        /// <param name="args">命令行参数,可以覆盖配置文件的信息</param>
        /// <returns>Config,环境名</returns>
        public static IConfiguration LoadConfig(ConfigEnvironment environment, string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            string configPath = GetEnvOrConfig(environment.EnvConfigPath, environment.ConfigPath);
            string configNamesStr = GetEnvOrConfig(environment.EnvConfigNames, environment.ConfigNames);

            // update
            if (!string.IsNullOrEmpty(configPath) || !string.IsNullOrEmpty(configNamesStr))
            {
                var configNames = configNamesStr.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                if (!string.IsNullOrEmpty(configPath))
                {
                    configPath = configPath.TrimEnd('/').TrimEnd('\\') + "/";
                    configBuilder.SetBasePath(configPath);
                }

                configNames?.ForEach(configName =>
                {
                    configBuilder.AddJsonFile(configName);
                });
            }

            // old
            else
            {
                string envName = GetEnvOrConfig(environment.EnvName, string.Empty);
                string pathName = GetEnvOrConfig(environment.EnvConfigPath, environment.ConfigPath);

                envName = string.IsNullOrEmpty(envName) ? string.Empty : "." + envName;
                pathName = string.IsNullOrEmpty(pathName) ? string.Empty : pathName + "/";
                string configName = $"{pathName}appsettings{envName}.json";

                configBuilder.AddJsonFile(configName);
            }

            configBuilder.AddEnvironmentVariables().AddCommandLine(args);

            return configBuilder.Build();
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
    }
}
