
using Casino.Domain.Models;

namespace Casino.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<Transaction> CreateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);
        
    }
}
