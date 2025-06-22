using Casino.Application.ModelsDTO;

namespace Casino.Application.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUsetById(int id, CancellationToken cancellationToken);
        Task<UserDTO> CreateUser(UserRegisterModel user, CancellationToken cancellationToken);
        Task<UserDTO> AuthenticationAsync(string userName, string password, CancellationToken cancellationToken);
        Task<decimal> GetUserBalance(int id, CancellationToken cancellationToken);
        Task<bool> FillBalance(int id, decimal amount, CancellationToken cancellationToken);
        Task<bool> WithdrawBalance(int id, decimal amount, CancellationToken cancellationToken);

    }
}
