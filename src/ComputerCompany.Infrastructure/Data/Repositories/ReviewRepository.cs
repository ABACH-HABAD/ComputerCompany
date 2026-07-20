using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;
using Microsoft.EntityFrameworkCore;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class ReviewRepository : BaseRepository, IReviewRepository
{
    private readonly CoreRepository<ReviewModel, ReviewEntity> _coreRepository;

    public ReviewRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Reviews);
        coreRepositoryBuilder.AddSelector(ReviewSelector.ToReviewModel);
        coreRepositoryBuilder.AddCreateFunc<ReviewEntity, ReviewModel>(model =>
        {
            return new ReviewEntity
            {
                SenderId = model.Sender.Id,
                Message = model.Message,
                Stars = model.Stars,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<ReviewEntity, ReviewModel>((entity, model) =>
        {
            entity.Message = model.Message;
            entity.SenderId = model.Sender.Id;
            entity.Stars = model.Stars;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<ReviewModel, ReviewEntity>();
    }

    public Task<ReviewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<ReviewModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(ReviewModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(ReviewModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);

    public async Task<ReviewModel?> GetByAccountIdAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        ReviewModel? reviewModel = await _applicationContext.Reviews
        .AsNoTracking()
        .Where(review => review.Sender.Id == accountId)
        .ToReviewModel()
        .FirstOrDefaultAsync(cancellationToken);

        return reviewModel;
    }

    public async Task<ReviewModel?> GetRandomAsync(CancellationToken cancellationToken = default)
    {
        ReviewModel? reviewModel = await _applicationContext.Reviews
        .AsNoTracking()
        .OrderBy(review => Guid.NewGuid())
        .ToReviewModel()
        .FirstOrDefaultAsync(cancellationToken);

        return reviewModel;
    }

    public async Task<List<ReviewModel>> GetRandomRangeAsync(int range, CancellationToken cancellationToken = default)
    {
        List<ReviewModel> reviews = await _applicationContext.Reviews
        .AsNoTracking()
        .OrderBy(review => Guid.NewGuid())
        .Take(range)
        .ToReviewModel()
        .ToListAsync(cancellationToken);

        return reviews;
    }
}