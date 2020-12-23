using Amo.Lib.Enums;
using Amo.Lib.Extensions;
using Amo.Lib.Model;

namespace Amo.Lib.CoreApi.Common
{
    public class Log : ILog
    {
        /// <summary>
        /// 指定输出配置,因为要伪装成json输出,所以其他字段都加了引号,message没加(message本身就是自定义的json),所以不能和其他输出共用
        /// </summary>
        private readonly NLog.Logger _logger = NLog.LogManager.GetLogger(ApiCommon.LogName);
        private bool hasError = false;

        public void ClearError()
        {
            hasError = false;
        }

        public void Error<T>(T log)
        {
            hasError = true;
            _logger.Error(log.ToJson());
        }

        public bool HasError()
        {
            return hasError;
        }

        public void Info<T>(T log)
        {
            _logger.Info(log.ToJson());
        }

        public void Trace<T>(T log)
        {
            _logger.Trace(log.ToJson());
        }

        public void Warn<T>(T log)
        {
            _logger.Warn(log.ToJson());
        }

        void ILog.Log<T>(T log)
        {
            if (log != null)
            {
                LogLevel level = LogLevel.Info;
                string msg = log.ToJson();
                if (log is LogEntity logEntity)
                {
                    level = Utils.GetLevel(logEntity.EventType);
                }

                if (level == LogLevel.Info)
                {
                    _logger.Info(msg);
                }
                else if (level == LogLevel.Debug)
                {
                    _logger.Debug(msg);
                }
                else if (level == LogLevel.Trace)
                {
                    _logger.Trace(msg);
                }
                else if (level == LogLevel.Warn)
                {
                    _logger.Warn(msg);
                }
                else if (level == LogLevel.Error)
                {
                    hasError = true;
                    _logger.Error(msg);
                }
                else if (level == LogLevel.Fatal)
                {
                    _logger.Fatal(msg);
                }
            }
        }
    }
}
