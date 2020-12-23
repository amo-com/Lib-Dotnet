using Consul;
using System;

namespace Amo.Lib.CoreApi.Common
{
    public class ConsulClientFactory
    {
        private static readonly object Locker = new object();
        private static IConsulClient consulClient;

        public static IConsulClient CreateClient(string address, string datacenter)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (consulClient == null)
            {
                lock (Locker)
                {
                    if (consulClient == null)
                    {
                        consulClient = new ConsulClient(x =>
                        {
                            x.Address = new Uri(address);

                            if (!string.IsNullOrEmpty(datacenter))
                            {
                                x.Datacenter = datacenter;
                            }
                        });
                    }
                }
            }

            return consulClient;
        }
    }
}
