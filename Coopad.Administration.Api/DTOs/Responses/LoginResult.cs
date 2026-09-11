namespace Coopad.Administration.Api.DTOs.Responses
{
    public class LoginResult
    {
        public string Token { get; set; } = null!;

        public UserDetailsDto User { get; set; } = null!;
    }
}
