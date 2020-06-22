using Amo.Lib.RestClient.Contexts;
using System.Text;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// query传参,只是声明了类型,为了区分类型
    /// </summary>
    public sealed class QueryAttribute : ParameterAttribute
    {
        public override void BeforeRequest(ApiActionContext context, ApiParameterDescriptor parameter)
        {
             context.RequestMessage.RequestUri = UsePathQuery(context.RequestMessage.RequestUri, parameter, Encoding.UTF8);
        }
    }
}
