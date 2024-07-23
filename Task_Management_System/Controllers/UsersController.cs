using Microsoft.AspNetCore.Mvc;
using Project.API.Interfaces;
using Project.API.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Project.API.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Project.API.Authorization;


namespace Project.API.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        private readonly IUserService _userService;
        public UsersController(ApplicationService applicationService, IUserService userService) 
        {
            _applicationService = applicationService;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        [Authorize(AuthenticationSchemes = "Basic")]
        public IActionResult Login()
        {
            var token = _applicationService.Userlogin(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            return Ok(token);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUsers()
        {
            IEnumerable<UserDto?> users = await _userService.GetAllUsers();
            return new JsonResult(new { status = true, users = users});
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto? user = await _userService.GetUserById(id);
            return new JsonResult(new { status = true, user = user });
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            bool result = await _applicationService.CreateUser(body);
            if (result)
                return Ok(new { status = true, message = "user created!" });
            else
                return new JsonResult(new { status = false, message = "there is problem when creating user" }) { StatusCode = 500};
        }

        [HttpDelete]
        [Route("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            bool result = await _userService.DeleteUser(userId);
            if (result)
                return Ok(new { status = true, message = "user deleted successfully from the system!" });
            else
                return new JsonResult(new { status = false, message = "there is problem when deleting user" }) { StatusCode = 500 };
        }
    }
}
