using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.PolicyConfig
{
    /// <summary>
    /// 降级(FallBack)
    /// </summary>
    public class FallBackPolicy : IPolicyConfig
    {
        public FallBackPolicy()
        {
        }

        public FallBackPolicy(Action action = null, Func<CancellationToken, Task> func = null)
        {
            this.Action = action;
            this.Func = func;
        }

        /// <summary>
        /// fallbackAction
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// 异步Task的fallbackAction
        /// </summary>
        public Func<CancellationToken, Task> Func { get; set; }

        public int Index => 1;

        public Policy Get()
        {
            Policy policyHandle = null;
            if (Action == null)
            {
                return policyHandle;
            }

            policyHandle = Policy.Handle<Exception>().Fallback(Action);

            return policyHandle;
        }

        public IAsyncPolicy GetAsync()
        {
            IAsyncPolicy policyHandle = null;
            if (Func == null)
            {
                return policyHandle;
            }

            policyHandle = Policy.Handle<Exception>().FallbackAsync(Func);

            return policyHandle;
        }

        public bool IsValid()
        {
            return Action != null || Func != null;
        }
    }
}
