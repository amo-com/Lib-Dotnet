using Amo.Lib.Model;

namespace Amo.Lib
{
    [Attributes.Autowired(Enums.ScopeType.Root)]
    public interface IRequestManager<TContext, TLog>
        where TLog : LogEntity
    {
        /// <summary>
        /// 获取Request下的IP，Url，UserAgent等信息
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>LogEntity</returns>
        TLog GetRequestLog(TContext context);
    }
}
