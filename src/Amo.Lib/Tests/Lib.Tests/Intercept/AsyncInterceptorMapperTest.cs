using Amo.Lib.Intercept;
using Amo.Lib.Tests.DataProxies;
using Castle.DynamicProxy;
using Xunit;

namespace Amo.Lib.Tests.Intercept
{
    public class AsyncInterceptorMapperTest
    {
        [Fact]
        public void ImplementTest()
        {
            var asyncInterceptor = new AsyncInterceptorMapper(new TestAsyncInterceptor(new ListLogger()));
            Assert.IsAssignableFrom<IInterceptor>(asyncInterceptor);
        }
    }
}
