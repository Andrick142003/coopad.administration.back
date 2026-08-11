using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync (string username);

        Task<User?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(string username);

        Task<User> CreateAsync(User user);
    }
}
