using Polly;
using System.Reflection;

namespace Amo.Lib.PolicyConfig
{
    /// <summary>
    /// Policy工厂,获取方法的Policy配置
    /// </summary>
    public interface IPolicyFactory
    {
        IAsyncPolicy GetAsyncPolicy(MethodInfo method);
        ISyncPolicy GetSyncPolicy(MethodInfo method);
    }
}
