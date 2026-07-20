using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class ReviewService(IValidator<ReviewModel> validator, IReviewRepository repository) :
BaseDataService<ReviewModel, IReviewRepository>(validator, repository, "отзыв"),
IReviewService
{
    public async Task<DataResult<ReviewModel>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        try
        {
            ReviewModel? reviewModel = await _repository.GetByAccountIdAsync(accountId, cancellationToken);

            if (reviewModel == null) return DataResult<ReviewModel>.Fail("Отзыв не найден");
            else return DataResult<ReviewModel>.Success(reviewModel);
        }
        catch (Exception ex)
        {
            return DataResult<ReviewModel>.Fail(ex.Message);
        }
    }

    public async Task<Result> SendReviewAsync(ReviewModel reviewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            ReviewModel? oldReview = await _repository.GetByAccountIdAsync(reviewModel.Sender.Id, cancellationToken);
            if (oldReview == null)
            {
                await _repository.AddAsync(reviewModel, cancellationToken);
                return Result.Success();
            }
            else
            {
                await _repository.UpdateAsync(reviewModel with { Id = oldReview.Id }, cancellationToken);
                return Result.Success();
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<DataResult<ReviewModel>> GetRandomAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            ReviewModel? reviewModel = await _repository.GetRandomAsync(cancellationToken);

            if (reviewModel == null) return DataResult<ReviewModel>.Fail("Отзыв не найден");
            else return DataResult<ReviewModel>.Success(reviewModel);
        }
        catch (Exception ex)
        {
            return DataResult<ReviewModel>.Fail(ex.Message);
        }
    }

    public async Task<DataResult<List<ReviewModel>>> GetRandomRangeAsync(int range, CancellationToken cancellationToken = default)
    {
        try
        {
            List<ReviewModel> reviews = await _repository.GetRandomRangeAsync(range, cancellationToken);

            if (reviews == null) return DataResult<List<ReviewModel>>.Fail("Отзыв не найден");
            else return DataResult<List<ReviewModel>>.Success(reviews);
        }
        catch (Exception ex)
        {
            return DataResult<List<ReviewModel>>.Fail(ex.Message);
        }
    }
}