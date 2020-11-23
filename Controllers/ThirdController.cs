using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace webapi_sample.Controllers
{
    [ApiController]
    [Route("Third")]
    public class ThirdController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        
        public ThirdController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Data()
        {
            using (var db = new Entity.PersonContext(_configuration))
            {
                StringBuilder sb = new StringBuilder();
                foreach(var item in await db.Persons.ToListAsync())
                {
                    sb.AppendFormat("User {0} : {1} {2}",item.Id,item.Name,item.Surname);
                    sb.AppendLine();
                }
                return Content(sb.ToString());
            }
        }
    }
       
}
