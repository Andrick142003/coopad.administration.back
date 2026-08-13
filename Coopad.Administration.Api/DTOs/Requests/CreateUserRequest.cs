namespace Coopad.Administration.Api.DTOs.Requests
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Email { get; set; }
    }
}
