using Coopad.Administration.Api.Authorization;
using Coopad.Administration.Api.Configuration;
using Coopad.Administration.Api.Constants;
using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Coopad.Administration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(
            IUserService userService,
            IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var result = await _userService
                .LoginAsync(
                    request.Username,
                    request.Password);

            if (result is null)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrectos."
                });
            }

            Response.Cookies.Append(
                AuthConstants.AccessTokenCookie,
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                   Expires = DateTimeOffset.UtcNow.AddMinutes(
                        _jwtSettings.ExpirationMinutes)
                });

            return Ok(new LoginResponse
            {
                User = result.User
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "No fue posible identificar al usuario."
                });
            }

            var user = await _userService
                .GetCurrentUserAsync(username);

            if (user is null)
            {
                return Unauthorized(new
                {
                    Success = false,
                    Message = "El usuario no existe o se encuentra inactivo."
                });
            }

            return Ok(new LoginResponse
            {
                User = user
            });
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(
                AuthConstants.AccessTokenCookie,
                new CookieOptions
                {
                    Path = "/",
                    Secure = false,
                    SameSite = SameSiteMode.Lax
                });

            return Ok(new
            {
                Success = true,
                Message = "Sesión cerrada correctamente."
            });
        }
    }
}