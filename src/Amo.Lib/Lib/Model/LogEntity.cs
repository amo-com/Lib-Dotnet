using Amo.Lib.Enums;
using System;

namespace Amo.Lib.Model
{
    /// <summary>
    /// 日志输出结构
    /// </summary>
    public class LogEntity
    {
        /// <summary>
        /// EventType
        /// </summary>
        public int EventType { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 延迟(ms)
        /// </summary>
        public long Latency { get; set; }

        /// <summary>
        /// Http请求方法
        /// </summary>
        public string RequestMethod { get; set; }

        /// <summary>
        /// Message描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 当前请求的Url(可能ApiUrl)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// guid
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 客户端引擎
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 前一级请求的Url.
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// 当前请求的页面Url
        /// </summary>
        public string CurrentUrl { get; set; }

        /// <summary>
        /// queryString请求参数
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// post,Put等请求的数据
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 自定义的LogKey,仅用于于ELK和UI Log进行记录对应
        /// </summary>
        public string LogKey { get; set; }

        /// <summary>
        /// 返回结果的状态
        /// </summary>
        public int StateCode { get; set; }

        /// <summary>
        /// exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}
