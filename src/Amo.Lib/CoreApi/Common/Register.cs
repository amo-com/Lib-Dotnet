using Amo.Lib.CoreApi.Models;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

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

            var serviceHost = options.ServiceHost ?? throw new ArgumentNullException(nameof(options.ServiceHost));
            var servicePort = options.ServicePort ?? throw new ArgumentNullException(nameof(options.ServicePort));
            var serviceName = options.ServiceName ?? throw new ArgumentNullException(nameof(options.ServiceName));
            var healthCheck = options.HealthCheck ?? throw new ArgumentNullException(nameof(options.HealthCheck));

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(options.DeregisterCriticalServiceAfter),
                Interval = TimeSpan.FromSeconds(options.Interval),
                HTTP = $"{options.Scheme}{serviceHost}:{servicePort}{healthCheck}",
                Timeout = TimeSpan.FromSeconds(options.Timeout)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = $"{serviceName}_{serviceHost}_{servicePort}",
                Name = serviceName,
                Address = options.Scheme + serviceHost,
                Port = Convert.ToInt32(servicePort),
                Tags = options.Tags, // 标签信息，服务发现的时候可以获取到的，负载均衡策略扩展的
                Meta = options.Meta
            };

            return registration;
        }
    }
}
