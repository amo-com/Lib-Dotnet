using Amo.Lib.RestClient.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.Tests
{
    public interface IPollyPolicyTestApi
    {
        [PollyRetry(3, 30)]
        Task<string> RetryTest1();

        [PollyTimeout(8)]
        Task<string> TimeoutTest1();

        [PollyBreaker(4, 5000)]
        Task<string> BreakerTest1();

        [PollyRetry(3, 30)]
        [PollyBreaker(4, 5000)]
        [PollyTimeout(5)]
        Task<string> Test4();
    }
}
