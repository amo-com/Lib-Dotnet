using Amo.Lib.RestClient.Contexts;
using Amo.Lib.RestClient.Defaults;
using Amo.Lib.RestClient.Extensions;
using System.Text;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// body传参,只是声明了类型,为了区分类型
    /// </summary>
    public sealed class BodyAttribute : ParameterAttribute
    {
        public override void BeforeRequest(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            context.RequestMessage.Content = new JsonContent(parameter.Value.ToJson(), Encoding.UTF8);
        }
    }
}
