using System.Net.Http;
using System.Text;

namespace Amo.Lib.RestClient.Defaults
{
    /// <summary>
    /// 表示http请求的json内容
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent"/> class.
        /// http请求的json内容
        /// </summary>
        /// <param name="json">json内容</param>
        /// <param name="encoding">编码</param>
        public JsonContent(string json, Encoding encoding)
            : base(json ?? string.Empty, encoding, MediaType)
        {
        }

        /// <summary>
        /// 获取对应的ContentType
        /// </summary>
        public static string MediaType => "application/json";
    }
}
