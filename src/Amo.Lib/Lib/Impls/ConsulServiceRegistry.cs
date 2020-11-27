using Amo.Lib.Enums;
using Consul;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amo.Lib.Impls
{
    public class ConsulServiceRegistry : IServiceRegistry
    {
        protected readonly IConsulClient client;

        public ConsulServiceRegistry(IConsulClient client)
        {
            this.client = client;
        }

        public int RetryCount { get; set; } = 3;

        public async Task ServiceRegister(AgentServiceRegistration service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            int temp = 0;
            do
            {
                try
                {
                    await client.Agent.ServiceRegister(service);
                    return;
                }
                catch (Exception ex)
                {
                    temp++;
                    if (temp < RetryCount)
                    {
                        // 阻塞线程3s
                        Thread.CurrentThread.Join(3000);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            while (true);
        }

        public async Task ServiceDeregister(string serviceId)
        {
            if (string.IsNullOrEmpty(serviceId))
            {
                throw new ArgumentNullException(nameof(serviceId));
            }

            await client.Agent.ServiceDeregister(serviceId);
        }

        public async Task SetServiceMaintenanceStatus(string serviceId, ServiceStatus status)
        {
            if (string.IsNullOrEmpty(serviceId))
            {
                throw new ArgumentNullException(nameof(serviceId));
            }

            if (ServiceStatus.DisableMaintenance.Equals(status))
            {
                await client.Agent.DisableServiceMaintenance(serviceId);
            }

            if (ServiceStatus.EnableMaintenance.Equals(status))
            {
                await client.Agent.EnableServiceMaintenance(serviceId, ServiceStatus.EnableMaintenance.ToString());
            }
        }
    }
}
