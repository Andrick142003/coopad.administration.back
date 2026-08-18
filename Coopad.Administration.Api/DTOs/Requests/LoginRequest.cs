using System.ComponentModel.DataAnnotations;
namespace Coopad.Administration.Api.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
