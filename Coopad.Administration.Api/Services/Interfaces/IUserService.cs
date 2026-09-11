using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface IUserService
    {

        Task<User?> GetByUsernameAsync(string username);

        Task<bool> ExistsAsync(string username);

        Task<User> CreateUserAsync(User user);

        Task<LoginResult?> LoginAsync(
            string username,
            string password);


        Task<UserDetailsDto?> GetCurrentUserAsync(
        string username);



    }
}
