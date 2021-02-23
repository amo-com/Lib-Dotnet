using Amo.Lib.RestClient.Attributes;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.Tests
{
    [HttpHost("http://api.dev/api")]
    [HttpRoutePrefix("test")]
    public interface IAubTestApi
    {
        [HttpPost]
        Task<HttpResponseMessage> PostAsync([Query]string name, [Query] int age);

        [HttpGet]
        Task<bool> RetryTest1([Query]string name, [Query] int age);
    }
}
