using Amo.Lib.PolicyConfig;
using Castle.DynamicProxy;
using Polly;
using System;
using System.Threading.Tasks;

namespace Amo.Lib.Intercept
{
    public class PolicyAsyncInterceptor : IAsyncInterceptor
    {
        private readonly IPolicyFactory _policyFactory;

        public PolicyAsyncInterceptor()
        {
        }

        public PolicyAsyncInterceptor(IPolicyFactory policyFactory)
        {
            this._policyFactory = policyFactory;
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = SignalWhenCompleteAsync(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = SignalWhenCompleteAsync<TResult>(invocation);
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            ISyncPolicy policy = _policyFactory?.GetSyncPolicy(invocation.Method);
            if (policy != null)
            {
                Action action = () => invocation.Proceed();
                policy.Execute(action);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private async Task SignalWhenCompleteAsync(IInvocation invocation)
        {
            IAsyncPolicy policy = _policyFactory?.GetAsyncPolicy(invocation.Method);
            if (policy != null)
            {
                await policy.ExecuteAsync(async () => await RunAsync(invocation).ConfigureAwait(false));
            }
            else
            {
                await RunAsync(invocation).ConfigureAwait(false);
            }
        }

        private async Task RunAsync(IInvocation invocation)
        {
            invocation.Proceed();
            var returnValue = (Task)invocation.ReturnValue;
            await returnValue;
        }

        private async Task<TResult> SignalWhenCompleteAsync<TResult>(IInvocation invocation)
        {
            IAsyncPolicy policy = _policyFactory?.GetAsyncPolicy(invocation.Method);
            if (policy != null)
            {
                return await policy.ExecuteAsync(async () => await RunAsync<TResult>(invocation).ConfigureAwait(false));
            }
            else
            {
                return await RunAsync<TResult>(invocation).ConfigureAwait(false);
            }
        }

        private async Task<TResult> RunAsync<TResult>(IInvocation invocation)
        {
            invocation.Proceed();
            var returnValue = (Task<TResult>)invocation.ReturnValue;
            return await returnValue;
        }
    }
}
