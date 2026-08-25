using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coopad.Administration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(
            IUserService userService)
        {
            _userService = userService;
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

            return Ok(new
            {
                Success = true,
                Message = "Autenticación exitosa.",
                Data = result
            });
        }
    }
}
