using ComputerCompany.Core.Models;

namespace ComputerCompany.Core.Abstractions.Repositories;

public interface IAccountRepository : IRepository<AccountModel>
{
    public Task<AccountModel?> GetByLoginAndPasswordAsync(string login, string password, CancellationToken cancellationToken = default);
    public Task<AccountModel?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    public Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken cancellationToken = default);
    public Task ChangePasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken = default);
    public Task AddAsync(AccountModel model, string password, CancellationToken cancellationToken = default);
}