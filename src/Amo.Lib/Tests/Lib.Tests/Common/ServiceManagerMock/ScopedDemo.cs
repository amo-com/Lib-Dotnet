using System;
using System.Collections.Generic;
using System.Text;

namespace Amo.Lib.Tests.Common.ServiceManagerMock
{
    public class ScopedDemo
    {
        [Attributes.Autowired(Enums.ScopeType.Scoped)]
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

        [Attributes.OverRide]
        public class Demo3 : Demo1, IDemo
        {
        }

        [Attributes.OverRide(Data.Sites.HPN)]
        public class Demo4 : Demo1, IDemo
        {
        }
    }
}
