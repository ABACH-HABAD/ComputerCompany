using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Data;

public interface IReviewService : IDataService<ReviewModel>
{
    public Task<DataResult<ReviewModel>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default);
    public Task<Result> SendReviewAsync(ReviewModel reviewModel, CancellationToken cancellationToken = default);
    public Task<DataResult<ReviewModel>> GetRandomAsync(CancellationToken cancellationToken = default);
    public Task<DataResult<List<ReviewModel>>> GetRandomRangeAsync(int range, CancellationToken cancellationToken = default);
}