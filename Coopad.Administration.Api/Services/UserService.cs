using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.DTOs.Responses;
using Coopad.Administration.Api.Infrastructure.ActiveDirectory;
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
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            SecurityDbContext context,
            IUnitOfWork unitOfWork,
            IActiveDirectoryService activeDirectoryService,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _context = context;
            _unitOfWork = unitOfWork;
            _activeDirectoryService = activeDirectoryService;
            _jwtService = jwtService;
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

        public async Task<LoginResponse?> LoginAsync(
            string username,
            string password)
        {

            var validCredentials =
            await _activeDirectoryService
             .ValidateCredentialsAsync(
            username,
            password);

            if (!validCredentials)
            {
                return null;
            }


            var activeDirectoryUser =
            await _activeDirectoryService
            .GetUserAsync(
            username,
            password);

            if (activeDirectoryUser is null)
            {
                return null;
            }



            var user = await _userRepository
                .GetByUsernameAsync(username);


            if (user is null)
            {
                user = new User
                {
                    Username = activeDirectoryUser.Username,

                    DisplayName =
                    activeDirectoryUser.DisplayName
                    ?? activeDirectoryUser.Username,

                    Email = activeDirectoryUser.Email,

                    IsActive = true
                };

                user = await CreateUserAsync(user);

                user = await _userRepository
                    .GetByUsernameAsync(username);
            }

            if (user is null)
            {
                throw new InvalidOperationException(
                    "No fue posible obtener el usuario.");
            }

            if (!user.IsActive)
            {
                throw new InvalidOperationException(
                    "El usuario se encuentra inactivo.");
            }


            var token = _jwtService.GenerateToken(user);

            return new LoginResponse
            {
                Token = token,

                User = new UserDetailsDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName,
                    Email = user.Email,

                    Roles = user.UserRoles
                        .Where(x => x.Role.IsActive)
                        .Select(x => x.Role.Name)
                        .Distinct()
                        .ToList(),

                    Permissions = user.UserRoles
                        .Where(x => x.Role.IsActive)
                        .SelectMany(x => x.Role.RolePermissions)
                        .Where(x => x.Permission.IsActive)
                        .Select(x => x.Permission.Name)
                        .Distinct()
                        .ToList()
                }
            };
        }
    }
}