using System;

namespace Amo.Lib
{
    /// <summary>
    /// Exception的EventType分析
    /// </summary>
    [Attributes.Autowired(Enums.ScopeType.Root)]
    public interface IExceptionAnalysis
    {
        /// <summary>
        /// 提取Exception的EventType
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>EventType,是否提取成功</returns>
        (int enentType, string message, bool isSuccess) GetEventType(Exception ex);
    }
}
