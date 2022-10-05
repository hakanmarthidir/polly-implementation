using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CircuitController : ControllerBase
    {
        public CircuitController()
        {
        }


        //CircuitBreaker
        [Route(("{isActive:bool}"))]
        [HttpGet]
        public IActionResult Get(bool isActive)
        {
            if (isActive == true)
            {
                return Ok("Ok");
            }
            else
            {
                throw new Exception("Error on API");
            }

        }
    }

}

