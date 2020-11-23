using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace webapi_sample.Controllers
{
    [ApiController]
    [Route("Second")]
    public class SecondController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        
        public SecondController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Data()
        {
            var client = new HttpClient();
            using (var request = new HttpRequestMessage(HttpMethod.Get,_configuration.GetValue<System.String>("thirdUrl")))
            {
                var response = await client.SendAsync(request);
                //throw new ApplicationException("My custom exception");
                return Content(await response.Content.ReadAsStringAsync());
            }
        }
    }
       
}
