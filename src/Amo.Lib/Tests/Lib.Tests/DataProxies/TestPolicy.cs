using Amo.Lib.Attributes;

namespace Amo.Lib.Tests.DataProxies
{
    [PollyBreaker(2, 200)]
    public class TestPolicy
    {
        [PollyRetry(3)]
        [PollyTimeout(100)]
        public void Test1()
        {
        }

        [PollyRetry(3, 500)]
        public void Test2()
        {
        }

        [PollyRetry(TimeSpans = new int[] { 100, 200 })]
        public void Test3()
        {
        }

        [PollyTimeout(Milliseconds = 300)]
        [PollyBreaker(2, 100)]
        public void Test4()
        {
        }
    }
}
