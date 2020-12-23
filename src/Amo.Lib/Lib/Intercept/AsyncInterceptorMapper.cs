using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;

namespace Amo.Lib.Intercept
{
    public class AsyncInterceptorMapper : IInterceptor
    {
        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(AsyncInterceptorMapper).GetMethod(nameof(HandleAsyncWithResult), BindingFlags.Static | BindingFlags.NonPublic);

        // 缓存返回类型的代理,优化性能
        private static readonly ConcurrentDictionary<Type, GenericAsyncHandler> GenericAsyncHandlers = new ConcurrentDictionary<Type, GenericAsyncHandler>();

        public AsyncInterceptorMapper(IAsyncInterceptor asyncInterceptor)
        {
            this.AsyncInterceptor = asyncInterceptor;
        }

        private delegate void GenericAsyncHandler(IInvocation invocation, IAsyncInterceptor asyncInterceptor);

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction,
        }

        public IAsyncInterceptor AsyncInterceptor { get; }

        /// <summary>
        /// 将默认的拦截转发到InterceptSynchronous,InterceptSynchronous{T},InterceptAsynchronous
        /// DI中注册的是IInvocation,通过本方法转发到IAsyncInterceptor的三个接口
        /// </summary>
        /// <param name="invocation">IInvocation</param>
        public void Intercept(IInvocation invocation)
        {
            MethodType methodType = GetMethodType(invocation.Method.ReturnType);
            switch (methodType)
            {
                case MethodType.AsyncAction:
                    AsyncInterceptor.InterceptAsynchronous(invocation);
                    return;
                case MethodType.AsyncFunction:
                    GetHandler(invocation.Method.ReturnType).Invoke(invocation, AsyncInterceptor);
                    return;
                default:
                    AsyncInterceptor.InterceptSynchronous(invocation);
                    return;
            }
        }

        private static MethodType GetMethodType(Type returnType)
        {
            if (returnType == typeof(void) || !typeof(Task).IsAssignableFrom(returnType))
            {
                return MethodType.Synchronous;
            }

            return returnType.GetTypeInfo().IsGenericType ? MethodType.AsyncFunction : MethodType.AsyncAction;
        }

        private static GenericAsyncHandler GetHandler(Type returnType)
        {
            GenericAsyncHandler handler = GenericAsyncHandlers.GetOrAdd(returnType, CreateHandler);
            return handler;
        }

        /// <summary>
        /// 生成returnType对应的泛型委托,否则type无法直接转为泛型T的方法
        /// </summary>
        /// <param name="returnType">返回类型</param>
        /// <returns>代理</returns>
        private static GenericAsyncHandler CreateHandler(Type returnType)
        {
            Type taskReturnType = returnType.GetGenericArguments()[0];
            MethodInfo method = HandleAsyncMethodInfo.MakeGenericMethod(taskReturnType);
            return (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
        }

        private static void HandleAsyncWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
        {
            asyncInterceptor.InterceptAsynchronous<TResult>(invocation);
        }
    }
}
