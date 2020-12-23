using Amo.Lib;
using Amo.Lib.Intercept;
using Castle.DynamicProxy;
using System.Diagnostics;

namespace Demo.Lib
{
    public class LoggerAsyncInterceptor : AsyncInterceptor<Stopwatch>
    {
        private readonly string _site;
        private readonly ILog _log;

        public LoggerAsyncInterceptor(IScoped scoped, ILog log)
        {
            this._site = scoped.GetScoped();
            this._log = log;
        }

        protected override Stopwatch StartingInvocation(IInvocation invocation)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        protected override void CompletedInvocation(IInvocation invocation, Stopwatch state)
        {
            state.Stop();
            LogTrace(invocation, state.ElapsedMilliseconds);
        }

        protected void LogTrace(IInvocation invocation, long latency)
        {
            _log?.Trace(new Models.InterceptLogEntity()
            {
                Site = _site,
                Latency = latency,
                ClassName = invocation.TargetType.FullName,
                MethodName = invocation.Method.Name,
            });
        }
    }
}
