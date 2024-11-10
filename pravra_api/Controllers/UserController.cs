using Microsoft.AspNetCore.Mvc;
using pravra_api.Extensions;
using pravra_api.Interfaces;
using pravra_api.Models;

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

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.UserId = Guid.NewGuid();
            var response = await _userService.CreateUser(user);
            return response.ToActionResult();
        }

        // Get a single user by UUID
        [HttpGet("getuserbyuserid")]
        public async Task<IActionResult> GetUserByUserId(Guid userId)
        {
            var response = await _userService.GetUserByUserId(userId);
            return response.ToActionResult();
        }

        // Get a single user
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser(string email, string password)
        {
            var response = await _userService.GetUser(email, password);
            return response.ToActionResult();
        }

        // Get all users
        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            return response.ToActionResult();
        }

        // Update an existing user
        [HttpPost("updateuser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            var response = await _userService.UpdateUser(request.UserId, request.FirstName, request.LastName, request.Mobile);
            return response.ToActionResult();
        }

        // Delete a user
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var response = await _userService.DeleteUser(userId);
            return response.ToActionResult();
        }

        [HttpPut("toggleuserstatus")]
        public async Task<IActionResult> ToggleUserStatus(Guid userId, bool status)
        {
            var response = await _userService.ToggleUserStatus(userId, status);
            return response.ToActionResult();
        }
    }
}