using Amo.Lib.Attributes;
using Amo.Lib.Extensions;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Amo.Lib.PolicyConfig
{
    public class PolicyFactory : IPolicyFactory
    {
        public Func<MethodInfo, List<int>> SortIndexsFun { get; set; }
        public Func<MethodInfo, List<IPolicyConfig>> PolicyConfigsFun { get; set; }

        public IAsyncPolicy GetAsyncPolicy(MethodInfo method)
        {
            return CreateAsyncPolicy(method);
        }

        public ISyncPolicy GetSyncPolicy(MethodInfo method)
        {
            return CreateSyncPolicy(method);
        }

        private IAsyncPolicy CreateAsyncPolicy(MethodInfo method)
        {
            List<IPolicyConfig> policyConfigs = GetPolicyConfigs(method);
            IAsyncPolicy policy = GetAsyncPolicyConfigs(policyConfigs);

            return policy;
        }

        private ISyncPolicy CreateSyncPolicy(MethodInfo method)
        {
            List<IPolicyConfig> policyConfigs = GetPolicyConfigs(method);
            ISyncPolicy policy = GetSyncPolicyConfigs(policyConfigs);

            return policy;
        }

        private List<IPolicyConfig> GetPolicyConfigs(MethodInfo method)
        {
            var factoryPolicies = PolicyConfigsFun != null ? PolicyConfigsFun(method) : null;
            var policyAttributes = method?
                 .GetAttributes<IPolicyAttribute>(true)
                 .ToList();
            var apiPolicies = policyAttributes?.Select(q => q.CreatePolicy()).ToList();
            var sortIndexs = SortIndexsFun != null ? SortIndexsFun(method) : null;
            List<IPolicyConfig> policyConfigs = CombinePolicyConfigs(factoryPolicies, apiPolicies, sortIndexs);

            return policyConfigs;
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

        private List<IPolicyConfig> CombinePolicyConfigs(List<IPolicyConfig> factoryPolicies, List<IPolicyConfig> apiPolicies, List<int> sortIndexs)
        {
            List<IPolicyConfig> policyConfigs = new List<IPolicyConfig>();

            if ((factoryPolicies == null || factoryPolicies.Count == 0)
                && (apiPolicies == null || apiPolicies.Count == 0))
            {
                return policyConfigs;
            }

            List<int> indexs = new List<int>();
            if (factoryPolicies != null)
            {
                indexs.AddRange(factoryPolicies.Select(q => q.Index).ToList());
            }

            if (apiPolicies != null)
            {
                indexs.AddRange(apiPolicies.Select(q => q.Index).ToList());
            }

            // 索引按升序排列,小的在最里面,最先执行
            indexs = indexs.Distinct().OrderBy(q => q).ToList();
            if (sortIndexs == null)
            {
                sortIndexs = indexs;
            }
            else
            {
                sortIndexs.AddRange(indexs.Except(sortIndexs).ToList());
            }

            foreach (var index in sortIndexs)
            {
                var factoryPolicy = factoryPolicies?.Find(q => q.Index == index);
                var apiPolicy = apiPolicies?.Find(q => q.Index == index);
                var policy = apiPolicy ?? factoryPolicy;
                if (policy != null)
                {
                    policyConfigs.Add(policy);
                }
            }

            return policyConfigs;
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
