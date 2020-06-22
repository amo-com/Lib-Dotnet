using Amo.Lib.Attributes;
using System.Threading.Tasks;

namespace Amo.Lib.Interface
{
    /// <summary>
    /// 委托代理,代理Task,统一封try,catch和失败处理
    /// </summary>
    [Autowired(Enums.ScopeType.Root)]
    public interface IBaseProxy
    {
        Task<(T data, bool state)> HandleTask<T>(Task<T> task, params string[] args);

        Task<bool> HandleTask(Task task, params string[] args);
    }
}
