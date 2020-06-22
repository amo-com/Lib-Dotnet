using Amo.Lib.RestClient.Contexts;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// header传参,只是声明了类型,为了区分类型
    /// </summary>
    public sealed class HeaderAttribute : ParameterAttribute
    {
        public override void BeforeRequest(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            context.RequestMessage.Headers.Remove(parameter.Name);
            context.RequestMessage.Headers.Add(parameter.Name, parameter.Value.ToString());
        }
    }
}
