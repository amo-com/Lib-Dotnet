using Amo.Lib.RestClient.Contexts;
using System;
using System.Collections.Generic;

namespace Amo.Lib.RestClient.Tests
{
    public class TestActionContext : ApiActionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestActionContext"/> class.
        /// 请求Api的上下文
        /// </summary>
        /// <param name="httpApiConfig">关联的HttpApiConfig</param>
        /// <param name="apiActionDescriptor">关联的ApiActionDescriptor</param>
        /// <param name="parameters">参数列表</param>
        /// <exception cref="ArgumentNullException">参数null异常</exception>
        public TestActionContext(HttpApiConfig httpApiConfig, ApiActionDescriptor apiActionDescriptor, IReadOnlyList<ApiParameterDescriptor> parameters)
            : base(httpApiConfig, apiActionDescriptor, parameters)
        {
        }
    }
}
