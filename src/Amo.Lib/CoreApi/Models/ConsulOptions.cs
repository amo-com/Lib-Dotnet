using System.Collections.Generic;

namespace Amo.Lib.CoreApi.Models
{
    public class ConsulOptions
    {
        /// <summary>
        /// consul服务地址
        /// </summary>
        public string ConsulAddress { get; set; }

        /// <summary>
        /// 数据中心
        /// </summary>
        public string Datacenter { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 跳转协议
        /// </summary>
        public string Scheme { get; set; } = "http://";

        /// <summary>
        /// 地址
        /// </summary>
        public string ServiceHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string ServicePort { get; set; }

        /// <summary>
        /// 健康检查地址
        /// </summary>
        public string HealthCheck { get; set; }

        /// <summary>
        /// 轮询时间间隔(单位:秒)
        /// </summary>
        public int Interval { get; set; } = 10;

        /// <summary>
        /// 缓存有效期(单位:秒)
        /// </summary>
        public int CacheTTL { get; set; } = 30;

        /// <summary>
        /// 在校验不通过多久之后注销服务(单位:秒)
        /// </summary>
        public int DeregisterCriticalServiceAfter { get; set; } = 60 * 10;

        /// <summary>
        /// 超时时间(单位:秒)
        /// </summary>
        public int Timeout { get; set; } = 5;

        /// <summary>
        /// Tags
        /// </summary>
        public string[] Tags { get; set; } = new string[] { };

        /// <summary>
        /// 元信息
        /// </summary>
        public Dictionary<string, string> Meta { get; set; }
    }
}
