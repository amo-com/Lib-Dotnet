using System;

namespace Amo.Lib.Tests.Common.ServiceManagerMock
{
    public class ScopedDemo
    {
        [Lib.Attributes.Autowired(Enums.ScopeType.Scoped)]
        public interface IDemo
        {
        }

        public class Demo1 : IDemo
        {
        }

        [Obsolete]
        public class Demo2 : IDemo
        {
        }

        [Lib.Attributes.OverRide]
        public class Demo3 : Demo1, IDemo
        {
        }

        [Lib.Attributes.OverRide(DataProxies.Sites.HPN)]
        public class Demo4 : Demo1, IDemo
        {
        }
    }
}
