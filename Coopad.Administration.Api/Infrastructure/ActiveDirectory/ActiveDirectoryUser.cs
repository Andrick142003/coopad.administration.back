namespace Coopad.Administration.Api.Infrastructure.ActiveDirectory
{
    public class ActiveDirectoryUser
    {
        public string Username { get; set; } = null!;

        public string? DisplayName { get; set; }

        public string? Email { get; set; }
    }
}
