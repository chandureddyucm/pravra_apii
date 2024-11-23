using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pravra_api.Extensions;
using pravra_api.Interfaces;
using pravra_api.Models;

namespace pravra_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        private string _userId;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor?.HttpContext?.User.FindFirst("UserId")?.Value ?? "User";

        }

        [AllowAnonymous]
        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.UserId = Guid.NewGuid();
            var response = await _userService.CreateUser(user);
            return response.ToActionResult();
        }

        // Get a single user by UUID
        [HttpGet("getuserbyuserid")]
        public async Task<IActionResult> GetUserByUserId()
        {
            var response = await _userService.GetUserByUserId(this._userId);
            return response.ToActionResult();
        }

        [AllowAnonymous]
        // Get a single user
        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var response = await _userService.Login(email, password);
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
            var response = await _userService.UpdateUser(this._userId, request.FirstName, request.LastName, request.Mobile);
            return response.ToActionResult();
        }

        // Delete a user
        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUser()
        {
            var response = await _userService.DeleteUser(this._userId);
            return response.ToActionResult();
        }

        [HttpPut("toggleuserstatus")]
        public async Task<IActionResult> ToggleUserStatus(bool status)
        {
            var response = await _userService.ToggleUserStatus(this._userId, status);
            return response.ToActionResult();
        }

        [AllowAnonymous]
        [HttpPut("addSubscription")]
        public async Task<IActionResult> AddSubscription(string email)
        {
            Subscription subscriber = new Subscription{SubscriptionId = Guid.NewGuid(), Email = email, IsActive = true};
            var response = await _userService.AddSubscription(subscriber);
            return response.ToActionResult();
        }

        [AllowAnonymous]
        [HttpGet("getAllSubscribers")]
        public async Task<IActionResult> GetAllSubscribers()
        {
            var response = await _userService.GetAllSubscribers();
            return response.ToActionResult();
        }
    }
}