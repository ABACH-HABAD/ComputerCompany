using ComputerCompany.Core.Models;

namespace ComputerCompany.Core.Abstractions.Repositories;

public interface IReviewRepository : IRepository<ReviewModel>
{
    public Task<ReviewModel?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    public Task<ReviewModel?> GetRandomAsync(CancellationToken cancellationToken = default);
    public Task<List<ReviewModel>> GetRandomRangeAsync(int range, CancellationToken cancellationToken = default);
}