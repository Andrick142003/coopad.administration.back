using Coopad.Administration.Api.Data;
using Coopad.Administration.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Coopad.Administration.Api.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SecurityDbContext _context;

        private IDbContextTransaction? _transaction;

        public UnitOfWork(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction =
                await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction is null)
                return;

            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction is null)
                return;

            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }
}
