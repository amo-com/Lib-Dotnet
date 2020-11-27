using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amo.Lib.Impls
{
    public class ConsulDiscoveryClient : IDiscoveryClient
    {
        protected readonly IConsulClient client;

        public ConsulDiscoveryClient(IConsulClient client)
        {
            this.client = client;
        }

        public async Task<Dictionary<string, string[]>> GetAllServices()
        {
            var result = await client.Catalog.Services();
            return result?.Response;
        }

        public async Task<List<string>> GetHealthServices(string serviceName)
        {
            var result = await client.Health.Service(serviceName, null, true);
            return result?.Response.Select(p => $"{p.Service.Address}:{p.Service.Port}").ToList();
        }
    }
}
