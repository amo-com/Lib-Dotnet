namespace Amo.Lib
{
    /// <summary>
    /// Log日志输出
    /// </summary>
    [Attributes.Autowired(Enums.ScopeType.Root)]
    public interface ILog
    {
        void Error<T>(T log);

        void Info<T>(T log);

        void Warn<T>(T log);

        /// <summary>
        /// 用于扩展按照EventTypeCode识别LogLevel进行记录
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="log">Data</param>
        void Log<T>(T log);

        // void Trace(string message);
        void Trace<T>(T log);

        /// <summary>
        /// 是否有Error记录(Error()和Log()中的Error都会标记为True)
        /// </summary>
        /// <returns>是否有Error记录</returns>
        bool HasError();

        /// <summary>
        /// 清除HasError的标记
        /// </summary>
        void ClearError();
    }
}
