using ComputerCompany.Core.Models;

namespace ComputerCompany.Core.Abstractions.Repositories;

public interface ISessionRepository : IRepository<SessionModel>
{
    public Task<SessionModel?> GetByTokenAsync(string refresh, string ip, CancellationToken cancellationToken = default);
    public Task<SessionModel?> GetByAccountAsync(Guid accountId, string ip, CancellationToken cancellationToken = default);
}