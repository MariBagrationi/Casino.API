using Casino.Application.Repositories;
using Casino.Domain.Models;
using Casino.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Casino.Infrastructure
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CasinoContext context) : base(context)
        {
        }

        public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            return await AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetUserAsync(string userName, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(userId, cancellationToken);
        }

        public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            return await UpdateAsync(user, cancellationToken);
        }
    }
}
