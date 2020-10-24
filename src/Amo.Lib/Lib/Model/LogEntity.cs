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
        /// exception
        /// </summary>
        public Exception Exception { get; set; }
    }
}
