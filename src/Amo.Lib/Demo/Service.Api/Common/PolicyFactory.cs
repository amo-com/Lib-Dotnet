using Amo.Lib.PolicyConfig;
using Polly;
using System.Collections.Generic;
using System.Reflection;

namespace Service.Api.Common
{
    /// <summary>
    /// 参考Amo.Lib.PolicyConfig.PolicyFactory
    /// 可以每个接口或者Class自定制策略,目前只使用简单的retry
    /// </summary>
    public class PolicyFactory : IPolicyFactory
    {
        private readonly IAsyncPolicy _asyncPolicy;
        private readonly ISyncPolicy _syncPolicy;

        public PolicyFactory()
        {
            PollyPolicy pollyPolicy = GetPollyPolicy();
            this._asyncPolicy = GetAsyncPolicyConfigs(pollyPolicy.Policies);
            this._syncPolicy = GetSyncPolicyConfigs(pollyPolicy.Policies);
        }

        public IAsyncPolicy GetAsyncPolicy(MethodInfo method)
        {
            return _asyncPolicy;
        }

        public ISyncPolicy GetSyncPolicy(MethodInfo method)
        {
            return _syncPolicy;
        }

        private PollyPolicy GetPollyPolicy()
        {
            PollyPolicy pollyPolicy = new PollyPolicy();
            pollyPolicy.RetryPolicy = new RetryPolicy(3);
            return pollyPolicy;
        }

        private IAsyncPolicy GetAsyncPolicyConfigs(List<IPolicyConfig> policies)
        {
            IAsyncPolicy policyWrap = null;

            policies?.ForEach(config =>
            {
                policyWrap = WrapAsync(config.GetAsync(), policyWrap);
            });

            return policyWrap;
        }

        private ISyncPolicy GetSyncPolicyConfigs(List<IPolicyConfig> policies)
        {
            ISyncPolicy policyWrap = null;

            policies?.ForEach(config =>
            {
                policyWrap = WrapSync(config.GetSync(), policyWrap);
            });

            return policyWrap;
        }

        private IAsyncPolicy WrapAsync(IAsyncPolicy outerPolicy, IAsyncPolicy innerPolicy)
        {
            if (innerPolicy != null && outerPolicy != null)
            {
                return outerPolicy.WrapAsync(innerPolicy);
            }
            else if (innerPolicy != null)
            {
                return innerPolicy;
            }
            else if (outerPolicy != null)
            {
                return outerPolicy;
            }

            return null;
        }

        private ISyncPolicy WrapSync(ISyncPolicy outerPolicy, ISyncPolicy innerPolicy)
        {
            if (innerPolicy != null && outerPolicy != null)
            {
                return outerPolicy.Wrap(innerPolicy);
            }
            else if (innerPolicy != null)
            {
                return innerPolicy;
            }
            else if (outerPolicy != null)
            {
                return outerPolicy;
            }

            return null;
        }
    }
}
