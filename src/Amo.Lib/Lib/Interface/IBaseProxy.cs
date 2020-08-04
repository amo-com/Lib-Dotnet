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
        /// <summary>
        /// 封装Task返回,统一处理报错和log记录
        /// </summary>
        /// <typeparam name="T">Task类型</typeparam>
        /// <param name="task">Task</param>
        /// <param name="args">附带的参数(多是Task方法的参数,用于Log记录调式)</param>
        /// <returns>返回结果</returns>
        Task<(T data, bool state)> HandleTask<T>(Task<T> task, params string[] args);

        /// <summary>
        /// 封装Task返回,统一处理报错和log记录
        /// </summary>
        /// <param name="task">Task</param>
        /// <param name="args">附带的参数(多是Task方法的参数,用于Log记录调式)</param>
        /// <returns>返回结果</returns>
        Task<bool> HandleTask(Task task, params string[] args);
    }
}
