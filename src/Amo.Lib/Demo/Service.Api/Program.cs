using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amo.Lib;
using Demo.Lib;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DefaultApi = Amo.Lib.CoreApi;

namespace Service.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DefaultApi.ApiCommon.ApiName = "Demo";
            DefaultApi.ApiCommon.CorsName = "AllowCors";
            DefaultApi.ApiCommon.LogName = "log";
            DefaultApi.ApiCommon.Scopeds = Sites.GetSites();
            DefaultApi.ApiCommon.NameSpaces = new List<string>() { "Service.Api", "Demo.Service" };
            DefaultApi.ApiCommon.XmlDocumentes = new List<string>() { "Demo.Service.xml", "Service.Api.xml", "Demo.Lib.xml" };

            Amo.Lib.Model.ConfigEnvironment environment = new Amo.Lib.Model.ConfigEnvironment();
            var config = Amo.Lib.ConfigManager.LoadConfig(environment, args);
            ServiceManager.InterceptType = Amo.Lib.Enums.InterceptType.Castle;

            DefaultApi.HostBuilder
                .CreateHostBuilder<Startup>(args, config)
                .Build()
                .Run();
        }
    }
}
