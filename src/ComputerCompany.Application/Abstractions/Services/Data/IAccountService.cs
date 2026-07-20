using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Data;

public interface IAccountService : IDataService<AccountModel>
{
    public Task<LoginResult> RegistrateAsync(string login, string password, string repeatPassword, string? ip = null, bool saveLoginData = true, CancellationToken cancellationToken = default);
    public Task<LoginResult> LoginAsync(string login, string password, string? ip = null, CancellationToken cancellationToken = default);
    public Task<Result> ResetPasswordAsync(Guid id, string oldPassword, string newPassword, bool forceChange = false, CancellationToken cancellationToken = default);
}