using Amo.Lib.Attributes;
using Demo.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    [Intercepts(typeof(LoggerAsyncInterceptor))]
    [Autowired(Amo.Lib.Enums.ScopeType.Scoped)]
    public interface ITest
    {
        void Work();
        void Wait();

        Task<int> GetIdAsync();

        Task<List<string>> GetNamesAsync();
    }
}
