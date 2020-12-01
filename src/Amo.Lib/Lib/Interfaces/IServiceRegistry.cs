using Amo.Lib.Enums;
using Consul;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib
{
    public interface IServiceRegistry
    {
        /// <summary>
        /// 尝试次数
        /// </summary>
        int RetryCount { get; set; }

        /// <summary>
        /// 注册一个服务到注册中心
        /// </summary>
        /// <param name="service">注册服务信息</param>
        /// <returns>Task</returns>
        Task ServiceRegister(AgentServiceRegistration service);

        /// <summary>
        /// 从注册中心注销一个
        /// </summary>
        /// <param name="serviceId">服务ID</param>
        /// <returns>Task</returns>
        Task ServiceDeregister(string serviceId);

        /// <summary>
        /// 设置服务维护状态
        /// </summary>
        /// <param name="serviceId">服务ID</param>
        /// <param name="status">服务维护状态</param>
        /// <returns>Task</returns>
        Task SetServiceMaintenanceStatus(string serviceId, ServiceStatus status);
    }
}
