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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            SecurityDbContext context,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _context = context;
            _unitOfWork = unitOfWork;
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
                    "El usuario ya existe.");
            }

            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.IsDefault &&
                    x.IsActive);

            if (defaultRole is null)
            {
                throw new InvalidOperationException(
                    "No existe un rol predeterminado activo.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var createdUser =
                    await _userRepository.CreateAsync(user);

                var userRole = new UserRole
                {
                    User = user,
                    RoleId = defaultRole.Id
                };

                _context.UserRoles.Add(userRole);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return createdUser;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }
    }
}
