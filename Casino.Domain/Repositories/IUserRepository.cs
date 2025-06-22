using Casino.Domain.Models;

namespace Casino.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
        Task<User?> GetUserAsync(string userName, CancellationToken cancellationToken);
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken);
    }
}
