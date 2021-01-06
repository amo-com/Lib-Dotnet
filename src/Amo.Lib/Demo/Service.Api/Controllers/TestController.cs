using Amo.Lib;
using Demo.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    }
}