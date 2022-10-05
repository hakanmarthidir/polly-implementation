using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeoutController : ControllerBase
    {
        public TimeoutController()
        {
        }


        //TimeOut
        [Route(("{delay:int}"))]
        [HttpGet]
        public IActionResult Get(int delay = 5000)
        {
            Thread.Sleep(delay);
            Console.WriteLine("Request for timeout " + delay);

            return Ok("Ok");
        }
    }

}

