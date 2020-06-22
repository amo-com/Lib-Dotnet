using System.Net.Http;

namespace Amo.Lib.RestClient.Attributes
{
    /// <summary>
    /// GET
    /// </summary>
    public sealed class HttpGetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// Get请求
        /// </summary>
        public HttpGetAttribute()
            : base(HttpMethod.Get)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGetAttribute"/> class.
        /// Get请求
        /// </summary>
        /// <param name="path">请求绝对或相对路径</param>
        public HttpGetAttribute(string path)
            : base(HttpMethod.Get, path)
        {
        }
    }
}
