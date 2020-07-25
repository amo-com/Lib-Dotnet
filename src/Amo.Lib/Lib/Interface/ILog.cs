using Amo.Lib.Attributes;
using Amo.Lib.Enums;
using Amo.Lib.Model;

namespace Amo.Lib
{
    /// <summary>
    /// Log日志输出
    /// </summary>
    [Autowired(ScopeType.Root)]
    public interface ILog
    {
        // void Error(string message);
        // void Error(LogEntity log);
        void Error<T>(T log);

        // void Info(string message);
        // void Info(LogEntity log);
        void Info<T>(T log);

        // void Warn(string message);
        // void Warn(LogEntity log);
        void Warn<T>(T log);

        void Log<T>(T log);

        // void Trace(string message);
        void Trace<T>(T log);

        bool HasError();

        void ClearError();
    }
}
