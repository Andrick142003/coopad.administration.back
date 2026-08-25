namespace Coopad.Administration.Api.DTOs.Responses
{
    public class LoginResponse
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public List<string> Roles { get; set; } = [];

        public List<string> Permissions { get; set; } = [];
    }
}
