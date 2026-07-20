using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public interface ISessionService : IDataService<SessionModel>
{
    public Task<LoginResult> RefreshTokensAsync(string refreshToken, string? ip = null, CancellationToken cancellationToken = default);
    public Task<DataResult<SessionModel>> GetByAccountAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default);
    public Task<Result> LogoutAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default);
}