using System;
using System.Collections.Generic;
using System.Text;

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
        public string Scheme { get; set; }

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
        public int Interval { get; set; }

        /// <summary>
        /// 缓存有效期(单位:秒)
        /// </summary>
        public int CacheTTL { get; set; }
    }
}
