namespace Amo.Lib.CoreApi.Models
{
    /// <summary>
    /// 对应Swagger的Microsoft.OpenApi.Models.ParameterLocation
    /// </summary>
    public enum ApiParameterLocation
    {
        /// <summary>
        /// Parameters that are appended to the URL.
        /// </summary>
        Query = 0,

        /// <summary>
        /// Custom headers that are expected as part of the request.
        /// </summary>
        Header = 1,

        /// <summary>
        /// Used together with Path Templating, where the parameter value is actually part of the operation's URL
        /// </summary>
        Path = 2,

        /// <summary>
        /// Used to pass a specific cookie value to the API.
        /// </summary>
        Cookie = 3
    }
}
