using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RetryController : ControllerBase
{
    public RetryController()
    {
    }

    //Retry
    [Route(("{id:int}"))]
    [HttpGet]
    public IActionResult GetRetry(int id)
    {
        Console.WriteLine("Request for retry : " + id);
        if (id == 1)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        return Ok("Ok");
    }

}

