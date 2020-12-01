using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib
{
    public interface ILoadBalancer
    {
        /// <summary>
        /// 获取服务请求地址
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>获取服务访问地址</returns>
        Task<string> ResolveServiceInstance(string serviceName);
    }
}
