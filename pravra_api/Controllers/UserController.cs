using Microsoft.AspNetCore.Mvc;

namespace Pravra.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("userDetails")]
        public IActionResult GetUserDetails()
        {
            var user = new
            {
                Id = 1,
                Name = "John Doe",
                Email = "john.doe@gmail.com",
                Age = 30
            };
            return Ok(user);
        }
    }
}