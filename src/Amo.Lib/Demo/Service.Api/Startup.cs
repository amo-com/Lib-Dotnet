using Amo.Lib;
using Amo.Lib.Intercept;
using Castle.DynamicProxy;
using Demo.Lib;
using Demo.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace Service.Api
{
    public class Startup : Amo.Lib.CoreApi.Startup
    {
        public Startup(IConfiguration configuration)
           : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            Sites.GetSites().ForEach(site =>
            {
                ServiceManager.GetServiceCollection(site)
                .AddSingleton<IAsyncInterceptor, LoggerAsyncInterceptor>();
            });
            ServiceManager.BuildServices();
            /*
            var a0 = ServiceManager.GetService<ITest>("AAA");
            a0.Work();
            a0.Wait();
            var s1 = await a0.GetIdAsync();
            var s2 = await a0.GetNamesAsync();
            */
            /*
             *ÐÔÄÜ·ÖÎö
            int ii = 0;
            var a0 = ServiceManager.GetService<ITest>("AAA");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (ii < 100000)
            {
                var a00 = ServiceManager.GetService<ITest>("AAA");
                ii++;
            }

            stopwatch.Stop();

            stopwatch.Stop();
            */
        }
    }
}
