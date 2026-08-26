using Coopad.Administration.Api.DTOs.Requests;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coopad.Administration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(
            string username)
        {
            var user = await _userService
                .GetByUsernameAsync(username);

            if (user is null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Usuario no encontrado."
                });
            }

            return Ok(new
            {
                Success = true,
                Data = user
            });
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser(
        CreateUserRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                DisplayName = request.DisplayName,
                Email = request.Email,
                IsActive = true
            };

            var createdUser = await _userService
                .CreateUserAsync(user);

            return CreatedAtAction(
                nameof(GetByUsername),
                new
                {
                    username = createdUser.Username
                },
                new
                {
                    Success = true,
                    Message = "Usuario creado correctamente.",
                    Data = createdUser
                }
            );
        }


        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                Success = true,
                User = User.Identity?.Name,
                Claims = User.Claims.Select(x => new
                {
                    x.Type,
                    x.Value
                })
            });
        }


    }
}
