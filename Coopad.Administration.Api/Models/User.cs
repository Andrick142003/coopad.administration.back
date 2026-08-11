namespace Coopad.Administration.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
    }
}
