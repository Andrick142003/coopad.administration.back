using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;
using Coopad.Administration.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Coopad.Administration.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SecurityDbContext _context;

        public UserService(
            IUserRepository userRepository,
            SecurityDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(
            string username)
        {
            return await _userRepository
                .GetByUsernameAsync(username);
        }

        public async Task<bool> ExistsAsync(
            string username)
        {
            return await _userRepository
                .ExistsAsync(username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var exists = await _userRepository
                .ExistsAsync(user.Username);

            if (exists)
            {
                throw new InvalidOperationException(
                    "El usuario ya existe."
                );
            }

            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.IsDefault &&
                    x.IsActive);

            if (defaultRole is null)
            {
                throw new InvalidOperationException(
                    "No existe un rol predeterminado activo."
                );
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var createdUser =
                    await _userRepository.CreateAsync(user);

                var userRole = new UserRole
                {
                    UserId = createdUser.Id,
                    RoleId = defaultRole.Id
                };

                _context.UserRoles.Add(userRole);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return createdUser;
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
