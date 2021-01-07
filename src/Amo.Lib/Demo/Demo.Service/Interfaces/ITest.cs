using Amo.Lib.Attributes;
using Amo.Lib.Intercept;
using Demo.Lib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    [Intercepts(typeof(LoggerAsyncInterceptor), typeof(PolicyAsyncInterceptor))]
    [Autowired(Amo.Lib.Enums.ScopeType.Scoped)]
    public interface ITest
    {
        void Work();
        void Wait();

        Task<int> GetIdAsync();

        Task<List<string>> GetNamesAsync();

        bool ThrowError();

        bool Retry();

        int Num();

        Task<string> ThrowErrorAsync();
    }
}
