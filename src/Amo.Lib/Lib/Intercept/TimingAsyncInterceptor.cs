using Castle.DynamicProxy;
using System.Diagnostics;

namespace Amo.Lib.Intercept
{
    /// <summary>
    /// 封装Stopwatch拦截,StartingTiming和CompletedTiming对应拦截执行前和执行后
    /// </summary>
    public abstract class TimingAsyncInterceptor : AsyncInterceptor<Stopwatch>
    {
        protected override Stopwatch StartingInvocation(IInvocation invocation)
        {
            StartingTiming(invocation);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        protected sealed override void CompletedInvocation(IInvocation invocation, Stopwatch state)
        {
            state.Stop();
            CompletedTiming(invocation, state);
        }

        protected abstract void StartingTiming(IInvocation invocation);

        protected abstract void CompletedTiming(IInvocation invocation, Stopwatch stopwatch);
    }
}
