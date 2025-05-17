using Application.DTO;
using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexora.Controllers.BaseControllerClass;
using Serilog;

namespace Nexora.Controllers.UserController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrUserDTO dto)
        {
            _logger.LogInformation("Register endpoint called");

            var result = await _userService.RegisterUser(dto);

            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            _logger.LogInformation("Login endpoint called");

            var result = await _userService.Login(dto);

            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("get/role/")]
        public async Task<IActionResult> GetRole()
        {
            var result= GetLoggedInUserRole();
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAccessToken(Guid userId)
        {
            _logger.LogInformation("Refresh Tocken  called");

            var result = await _userService.RefreshAccessTocken (userId);

            if (result.StatusCode == 200)
                return Ok(result);

            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("get-id")]
        public async Task<IActionResult> GetIdInCookies()
        {
            var userId =  GetLoggedInUserId();


            if (userId.HasValue)
            {
            
                return Ok(userId.Value);
            }
            return Unauthorized(null);

        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

            return Ok(new { message = "Logged out successfully" });
        }

    }
}
