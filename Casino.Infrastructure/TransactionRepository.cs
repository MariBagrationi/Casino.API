using Casino.Domain.Models;
using Casino.Domain.Repositories;
using Casino.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Casino.Infrastructure
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(CasinoContext context) : base(context)
        {
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            await base.AddAsync(transaction, cancellationToken);
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(t => t.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
