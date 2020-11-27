using Amo.Lib.CoreApi.Models;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.CoreApi.Common
{
    public static class Register
    {
        public static IApplicationBuilder StartRegister(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetRequiredService<IOptions<ConsulOptions>>();
            var consulOptions = options?.Value;
            if (consulOptions == null)
            {
                throw new ArgumentNullException(nameof(consulOptions));
            }

            // 请求注册的 Consul 地址
            var consulClient = ConsulClientFactory.CreateClient(consulOptions.ConsulAddress, consulOptions.Datacenter);
            var registration = CreateRegistration(consulOptions);
            var serviceRegistry = app.ApplicationServices.GetRequiredService<IServiceRegistry>();
            if (serviceRegistry != null)
            {
                serviceRegistry.ServiceRegister(registration).Wait();
            }

            // 获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait(); // 服务停止时取消注册
            });

            return app;
        }

        public static AgentServiceRegistration CreateRegistration(ConsulOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var scheme = options.Scheme ?? "http://";
            var serviceHost = options.ServiceHost ?? throw new ArgumentNullException("ServiceHost");
            var servicePort = options.ServicePort ?? throw new ArgumentNullException("ServicePort");
            var serviceName = options.ServiceName ?? throw new ArgumentNullException("ServiceName");
            var healthCheck = options.HealthCheck ?? throw new ArgumentNullException("HealthCheck");
            var interval = options.Interval > 0 ? options.Interval : 10;

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(15),
                Interval = TimeSpan.FromSeconds(interval),
                HTTP = $"{scheme}{serviceHost}:{servicePort}{healthCheck}",
                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = $"{serviceName}_{serviceHost}_{servicePort}",
                Name = serviceName,
                Address = scheme + serviceHost,
                Port = Convert.ToInt32(servicePort),
                Tags = new string[] { } // 标签信息，服务发现的时候可以获取到的，负载均衡策略扩展的
            };

            return registration;
        }
    }
}
