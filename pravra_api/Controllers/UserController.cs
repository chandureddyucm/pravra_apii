// using Microsoft.AspNetCore.Mvc;

// namespace pravra_api.Controllers
// {
//     [Route("api/user")]
//     [ApiController]
//     public class UserController : ControllerBase
//     {
//         [HttpGet("userDetails")]
//         public IActionResult GetUserDetails()
//         {
//             var user = new
//             {
//                 Id = 1,
//                 Name = "John Doe",
//                 Email = "john.doe@gmail.com",
//                 Age = 30
//             };
//             return Ok(user);
//         }
//     }
// }
using Microsoft.AspNetCore.Mvc;
using pravra_api.Interfaces;
using pravra_api.Models;
using System;
using System.Threading.Tasks;

namespace pravra_api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.UUID = Guid.NewGuid(); // Generate UUID
            await _userService.CreateUser(user);
            return CreatedAtAction(nameof(CreateUser), new { id = user.UUID }, user);
        }
    }
}