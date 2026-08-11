using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);

        Task<User?> GetByUsernameAsync(string username);

        Task<bool> ExistsAsync(string username);
    }
}
