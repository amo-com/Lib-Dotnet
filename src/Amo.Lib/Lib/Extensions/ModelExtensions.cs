using Amo.Lib.Enums;
using Amo.Lib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Extensions
{
    public static class ModelExtensions
    {
        #region LogEntity
        public static T AddEx<T>(this T log, Exception ex)
            where T : LogEntity
        {
            log.Exception = ex;
            return log;
        }

        public static T SetLatency<T>(this T log, long latency)
            where T : LogEntity
        {
            log.Latency = latency;
            return log;
        }

        #endregion
    }
}
