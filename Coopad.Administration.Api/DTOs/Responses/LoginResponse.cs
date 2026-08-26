namespace Coopad.Administration.Api.DTOs.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;

        public UserDetailsDto User { get; set; } = null!;
    }
}
