using Amo.Lib.Model;

namespace Amo.Lib
{
    [Attributes.Autowired(Enums.ScopeType.Root)]
    public interface IRequestManager
    {
        /// <summary>
        /// 获取Request下的IP，Url，UserAgent等信息
        /// </summary>
        /// <typeparam name="TContext">TContext</typeparam>
        /// <typeparam name="TLog">TLog</typeparam>
        /// <param name="context">Context</param>
        /// <returns>Log</returns>
        TLog GetRequestLog<TContext, TLog>(TContext context)
            where TLog : LogEntity, new();
    }
}
