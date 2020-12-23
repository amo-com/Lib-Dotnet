using Castle.DynamicProxy;
using System.Threading.Tasks;

namespace Amo.Lib.Intercept
{
    /// <summary>
    /// 支持前后拦截的封装,可以统一实现StartingInvocation和CompletedInvocation
    /// </summary>
    /// <typeparam name="TState">用于操作拦截前后的Class</typeparam>
    public abstract class AsyncInterceptor<TState> : IAsyncInterceptor
        where TState : class
    {
        public void InterceptAsynchronous(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteAsync(invocation, state);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            TState state = Proceed(invocation);
            invocation.ReturnValue = SignalWhenCompleteAsync<TResult>(invocation, state);
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            TState state = Proceed(invocation);

            CompletedInvocation(invocation, state, invocation.ReturnValue);
        }

        protected abstract TState StartingInvocation(IInvocation invocation);
        protected virtual void CompletedInvocation(IInvocation invocation, TState state)
        {
        }

        protected virtual void CompletedInvocation(IInvocation invocation, TState state, object returnValue)
        {
            CompletedInvocation(invocation, state);
        }

        private TState Proceed(IInvocation invocation)
        {
            TState state = StartingInvocation(invocation);
            invocation.Proceed();
            return state;
        }

        private async Task SignalWhenCompleteAsync(IInvocation invocation, TState state)
        {
            var returnValue = (Task)invocation.ReturnValue;
            await returnValue.ConfigureAwait(false);
            CompletedInvocation(invocation, state, null);
        }

        private async Task<TResult> SignalWhenCompleteAsync<TResult>(IInvocation invocation, TState state)
        {
            var returnValue = (Task<TResult>)invocation.ReturnValue;
            TResult result = await returnValue.ConfigureAwait(false);
            CompletedInvocation(invocation, state, result);

            return result;
        }
    }
}
