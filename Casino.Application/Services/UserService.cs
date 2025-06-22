using Casino.Application.ModelsDTO;
using Casino.Application.Repositories;
using Casino.Domain.Models;
using Casino.Domain.Repositories;
using Casino.Domain.Security;
using Mapster;

namespace Casino.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDTO> AuthenticationAsync(string userName, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(userName, cancellationToken);
            if (user == null)
                throw new Exception("User not found.");

            var pass = PasswordGenerator.GenerateHash(password);
            if (user.Password != pass)
                throw new Exception("password is incorrect");

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> CreateUser(UserRegisterModel user, CancellationToken cancellationToken)
        {
            var pass = PasswordGenerator.GenerateHash(user.Password);
            var entity = user.Adapt<User>();
            entity.Password = pass;

            var created = await _userRepository.CreateUserAsync(entity, cancellationToken);

            return created.Adapt<UserDTO>();
        }

        public async Task<bool> FillBalance(int id, decimal amount, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
            user.Balance += amount;
            await _userRepository.UpdateUserAsync(user, cancellationToken);
            return true;
        }

        public async Task<decimal> GetUserBalance(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
            if (user == null)
                throw new Exception("User not found.");

            return await Task.FromResult(user.Balance);
        }

        public async Task<UserDTO> GetUsetById(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
            if (user == null)
                throw new Exception("User not found.");

            return user.Adapt<UserDTO>();
        }

        public async Task<bool> WithdrawBalance(int userId, decimal amount, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

            if (user == null || user.Balance < amount)
                return false;

            try
            {
                user.Balance -= amount;

                var transactionEntity = new Transaction
                {
                    UserId = user.Id,
                    Amount = -amount,
                    Date = DateTime.UtcNow,
                };

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }

            return true;
        }

    }
}
