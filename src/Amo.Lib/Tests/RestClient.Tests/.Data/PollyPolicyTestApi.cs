using Amo.Lib.RestClient.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Amo.Lib.RestClient.Tests
{
    [PollyRetry(2)]
    public class PollyPolicyTestApi : IPollyPolicyTestApi
    {
        public Task<string> BreakerTest1()
        {
            throw new NotImplementedException();
        }

        [PollyBreaker(2, 500)]
        public Task<string> RetryTest1()
        {
            throw new NotImplementedException();
        }

        public Task<string> Test4()
        {
            throw new NotImplementedException();
        }

        public Task<string> TimeoutTest1()
        {
            throw new NotImplementedException();
        }
    }
}
