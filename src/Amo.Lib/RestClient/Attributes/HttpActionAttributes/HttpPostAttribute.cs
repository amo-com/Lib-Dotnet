using System.Net.Http;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// Post请求
    /// 不可继承
    /// </summary>
    public sealed class HttpPostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class.
        /// Post请求
        /// </summary>
        public HttpPostAttribute()
            : base(HttpMethod.Post)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpPostAttribute"/> class.
        /// Post请求
        /// </summary>
        /// <param name="path">请求绝对或相对路径</param>
        public HttpPostAttribute(string path)
            : base(HttpMethod.Post, path)
        {
        }
    }
}
