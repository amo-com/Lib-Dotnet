using Amo.Lib.Attributes;
using Amo.Lib.PolicyConfig;
using Amo.Lib.Tests.DataProxies;
using Polly;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Amo.Lib.Tests.PolicyConfig
{
    public class PolicyFactoryTest
    {
        [Fact]
        public void Test1()
        {
            PolicyFactory factory = new PolicyFactory();
            Assert.Null(factory.PolicyConfigsFun);
            Assert.Null(factory.SortIndexsFun);
        }

        [Fact]
        public void GetAsyncPolicyTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "GetAsyncPolicy";

            var policy = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(policy);

            MethodInfo method = GetPrivateMethod(typeof(TestPolicy), "Test1");
            var policy2 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.NotNull(policy2);
        }

        [Fact]
        public void GetSyncPolicyTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "GetSyncPolicy";

            var policy = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(policy);

            MethodInfo method = GetPrivateMethod(typeof(TestPolicy), "Test1");
            var policy2 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.NotNull(policy2);
        }

        [Fact]
        public void CreateAsyncPolicyTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "CreateAsyncPolicy";

            var policy = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(policy);

            MethodInfo method = GetPrivateMethod(typeof(TestPolicy), "Test1");
            var policy2 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.NotNull(policy2);
        }

        [Fact]
        public void CreateSyncPolicyTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "CreateSyncPolicy";

            var policy = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(policy);

            MethodInfo method = GetPrivateMethod(typeof(TestPolicy), "Test1");
            var policy2 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.NotNull(policy2);
        }

        [Fact]
        public void GetPolicyConfigsTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "GetPolicyConfigs";
            List<IPolicyConfig> policies = new List<IPolicyConfig>();

            var configs = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Empty(configs);

            MethodInfo method = GetPrivateMethod(typeof(TestPolicy), "Test1");
            var configs2 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.NotEmpty(configs2);
            Assert.Equal(2, configs2.Count);

            MethodInfo method2 = GetPrivateMethod(typeof(TestPolicy), "Test2");
            var configs3 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method2 });
            Assert.NotEmpty(configs3);
            Assert.Single(configs3);

            factory.PolicyConfigsFun = (method) => { return new List<IPolicyConfig>() { new RetryPolicy(5, 200) }; };
            var configs4 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.Equal(2, configs4.Count);
            Assert.True(configs4[0] is RetryPolicy);

            factory.SortIndexsFun = (method) => { return new List<int>() { 5, 4, 3, 2, 1 }; };
            var configs5 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { method });
            Assert.True(configs5[0] is TimeoutPolicy);
        }

        [Fact]
        public void GetAsyncPolicyConfigsTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "GetAsyncPolicyConfigs";
            List<IPolicyConfig> policies = new List<IPolicyConfig>();

            var configs = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(configs);

            policies.Add(new RetryPolicy());
            var configs2 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { policies });
            Assert.Null(configs2);

            policies.Add(new RetryPolicy(5));
            var configs3 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { policies });
            Assert.NotNull(configs3);
        }

        [Fact]
        public void GetSyncPolicyConfigsTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "GetSyncPolicyConfigs";
            List<IPolicyConfig> policies = new List<IPolicyConfig>();

            var configs = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { null });
            Assert.Null(configs);

            policies.Add(new RetryPolicy());
            var configs2 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { policies });
            Assert.Null(configs2);

            policies.Add(new RetryPolicy(5));
            var configs3 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[1] { policies });
            Assert.NotNull(configs3);
        }

        [Fact]
        public void CombinePolicyConfigsTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "CombinePolicyConfigs";

            var configs = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { null, null, null });
            Assert.Empty(configs);

            var factoryPolicies = new List<IPolicyConfig>() { };
            var apiPolicies = new List<IPolicyConfig>() { };
            var sortIndexs = new List<int>();

            var configs2 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { factoryPolicies, apiPolicies, sortIndexs });
            Assert.Empty(configs2);

            RetryPolicy retryPolicy = new RetryPolicy(5);
            factoryPolicies.Add(retryPolicy);
            var configs3 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { factoryPolicies, apiPolicies, sortIndexs });
            Assert.NotEmpty(configs3);
            Assert.Single(configs3);

            TimeoutPolicy timeoutPolicy = new TimeoutPolicy(300);
            apiPolicies.Add(timeoutPolicy);
            var configs4 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { factoryPolicies, apiPolicies, sortIndexs });
            Assert.NotEmpty(configs4);
            Assert.Equal(2, configs4.Count);
            Assert.Equal(retryPolicy, configs4[0]);

            RetryPolicy apiRetryPolicy = new RetryPolicy(8);
            apiPolicies.Add(apiRetryPolicy);
            var configs5 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { factoryPolicies, apiPolicies, sortIndexs });
            Assert.Equal(2, configs5.Count);
            Assert.Equal(apiRetryPolicy, configs5[0]);

            sortIndexs = new List<int>() { 5, 4, 3, 2, 1 };
            var configs6 = (List<IPolicyConfig>)Helper.RunInstanceMethod(type, methodName, factory, new object[3] { factoryPolicies, apiPolicies, sortIndexs });
            Assert.NotEmpty(configs6);
            Assert.Equal(timeoutPolicy, configs6[0]);
        }

        [Fact]
        public void WrapAsyncTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "WrapAsync";
            var retryPolicy = Policy.Handle<Exception>().RetryAsync(2);
            var timeoutPolicy = Policy.TimeoutAsync(200);

            var policy = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { null, null });
            Assert.Null(policy);

            var policy2 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { retryPolicy, null });
            Assert.Equal(retryPolicy, policy2);

            var policy3 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { null, timeoutPolicy });
            Assert.Equal(timeoutPolicy, policy3);

            var policy4 = (IAsyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { retryPolicy, timeoutPolicy });
            Assert.NotNull(policy4);
            Assert.NotEqual(timeoutPolicy, policy4);
            Assert.NotEqual(retryPolicy, policy4);
            Assert.Equal(retryPolicy, ((Polly.Wrap.AsyncPolicyWrap)policy4).Outer);
            Assert.Equal(timeoutPolicy, ((Polly.Wrap.AsyncPolicyWrap)policy4).Inner);
        }

        [Fact]
        public void WrapSyncTest()
        {
            var type = typeof(PolicyFactory);
            PolicyFactory factory = new PolicyFactory();
            string methodName = "WrapSync";
            var retryPolicy = Policy.Handle<Exception>().Retry(2);
            var timeoutPolicy = Policy.Timeout(200);

            var policy = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { null, null });
            Assert.Null(policy);

            var policy2 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { retryPolicy, null });
            Assert.Equal(retryPolicy, policy2);

            var policy3 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { null, timeoutPolicy });
            Assert.Equal(timeoutPolicy, policy3);

            var policy4 = (ISyncPolicy)Helper.RunInstanceMethod(type, methodName, factory, new object[2] { retryPolicy, timeoutPolicy });
            Assert.NotNull(policy4);
            Assert.NotEqual(retryPolicy, policy4);
            Assert.NotEqual(timeoutPolicy, policy4);
            Assert.Equal(retryPolicy, ((Polly.Wrap.PolicyWrap)policy4).Outer);
            Assert.Equal(timeoutPolicy, ((Polly.Wrap.PolicyWrap)policy4).Inner);
        }

        private MethodInfo GetPrivateMethod(Type type, string methodName)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return type.GetMethod(methodName, eFlags);
        }
    }
}
