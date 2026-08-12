using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.Models;
using Coopad.Administration.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Coopad.Administration.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SecurityDbContext _context;

        public UserRepository(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(
            string username)
        {
            return await _context.Users
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                        .ThenInclude(x => x.RolePermissions)
                            .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x =>
                    x.Username == username);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.Id == id);
        }

        public async Task<bool> ExistsAsync(
            string username)
        {
            return await _context.Users
                .AnyAsync(x =>
                    x.Username == username);
        }



        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);

            return user;
        }

    }
}
