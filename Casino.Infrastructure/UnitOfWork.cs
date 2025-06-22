using Casino.Domain.Repositories;
using Casino.Persistance.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Casino.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CasinoContext _context;

        public UnitOfWork(CasinoContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken) =>
            await _context.SaveChangesAsync(cancellationToken);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
            await _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken) =>
            await transaction.CommitAsync(cancellationToken);

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken) =>
            await transaction.RollbackAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
    }
}
