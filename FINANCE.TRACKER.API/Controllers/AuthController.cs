using FINANCE.TRACKER.API.Data;
using FINANCE.TRACKER.API.Models.Auth;
using FINANCE.TRACKER.API.Models.UserManager;
using FINANCE.TRACKER.API.Services.Interfaces.Jwt;
using FINANCE.TRACKER.API.Services.Interfaces.UserManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FINANCE.TRACKER.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;

        public AuthController(
            AppDbContext context, 
            IJwtService jwtService, 
            IOptions<JwtSettings> jwtOptions,
            IUserService userService
        )
        {
            _context = context;
            _jwtService = jwtService;
            _jwtSettings = jwtOptions.Value;
            _userService = userService;
        }

        public class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Username) || string.IsNullOrWhiteSpace(request?.Password))
                return BadRequest(new { message = "Username and password are required." });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Unauthorized(new { message = "Username/password is invalid!" });   

            var identityHasher = new PasswordHasher<UserModel>();
            var verification = identityHasher.VerifyHashedPassword(user, user.Password, request.Password);

            if (verification == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Username/password is invalid!" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new { 
                token,
                tokenExp = _jwtSettings.ExpiresInMinutes,
                modules = await _userService.GetUserModules(user.UserId)
            });
        }
    }
}
