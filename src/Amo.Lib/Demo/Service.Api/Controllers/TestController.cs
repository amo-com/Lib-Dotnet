using Amo.Lib;
using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Service.Api.Controllers
{
    [Route("test")]
    [EnableCors("AllowCors")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly ITest service;

        public TestController()
        {
            this.service = ServiceManager.GetService<ITest>("AAA");
        }

        [HttpGet]
        [Route("demo")]
        public async Task TestDemo()
        {
            await service.GetIdAsync();
        }

        [HttpGet]
        [Route("test-retry")]
        public bool TestRetry()
        {
            try
            {
                return service.ThrowError();
            }
            catch (Exception ex)
            {
                var num = service.Num();
                throw ex;
            }
        }

        [HttpGet]
        [Route("test-retry-default")]
        public bool TestRetryDefault()
        {
            return service.Retry();
        }

        [HttpGet]
        [Route("test-retry-2")]
        public async Task<string> TestRetry2()
        {
            return await service.ThrowErrorAsync();
        }
    }
}