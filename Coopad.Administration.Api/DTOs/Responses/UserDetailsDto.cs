namespace Coopad.Administration.Api.DTOs.Responses
{
    public class UserDetailsDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Email { get; set; }

        public List<string> Roles { get; set; } = [];

        public List<string> Permissions { get; set; } = [];
    }
}
