using Amo.Lib.Intercept;
using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Amo.Lib.Tests.DataProxies
{
    public class TestAsyncInterceptor : IAsyncInterceptor
    {
        private readonly ListLogger _log;

        public TestAsyncInterceptor(ListLogger log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            LogInterceptStart(invocation);
            invocation.Proceed();
            LogInterceptEnd(invocation);
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = LogInterceptAsynchronous(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = LogInterceptAsynchronous<TResult>(invocation);
        }

        private async Task LogInterceptAsynchronous(IInvocation invocation)
        {
            LogInterceptStart(invocation);
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task.ConfigureAwait(false);
            LogInterceptEnd(invocation);
        }

        private async Task<TResult> LogInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            LogInterceptStart(invocation);
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            TResult result = await task.ConfigureAwait(false);
            LogInterceptEnd(invocation);
            return result;
        }

        private void LogInterceptStart(IInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptStart");
        }

        private void LogInterceptEnd(IInvocation invocation)
        {
            _log.Add($"{invocation.Method.Name}:InterceptEnd");
        }
    }
}
