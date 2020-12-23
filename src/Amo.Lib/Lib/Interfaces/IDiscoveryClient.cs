using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amo.Lib
{
    public interface IDiscoveryClient
    {
        /// <summary>
        /// 获取所有服务节点
        /// </summary>
        /// <returns>所有服务访问列表</returns>
        Task<Dictionary<string, string[]>> GetAllServices();

        /// <summary>
        /// 获取某个服务下的所有健康节点
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>服务访问列表</returns>
        Task<List<string>> GetHealthServices(string serviceName);
    }
}
