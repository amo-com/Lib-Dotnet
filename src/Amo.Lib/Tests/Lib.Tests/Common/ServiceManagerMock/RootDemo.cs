namespace Amo.Lib.Tests.Common.ServiceManagerMock
{
    /// <summary>
    /// RootDemo,单纯接口
    /// 无标记
    /// </summary>
    public class RootDemo
    {
        [Lib.Attributes.Autowired(Enums.ScopeType.Root)]
        public interface IDemo
        {
        }

        public class Demo1 : IDemo
        {
        }

        [System.Obsolete]
        public class Demo2 : IDemo
        {
        }

        [Lib.Attributes.OverRide]
        public class Demo3 : Demo1, IDemo
        {
        }
    }
}
