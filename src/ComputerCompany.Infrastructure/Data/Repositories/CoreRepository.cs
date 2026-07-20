using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Repositories;

internal class CoreRepository<TModel, TEntity> : IRepository<TModel>
where TModel : BaseModel
where TEntity : BaseEntity
{
    internal required ApplicationContext ApplicationContext { private get; init; }
    internal required DbSet<TEntity> Set { private get; init; }
    internal required Expression<Func<TEntity, TModel>> Selector { private get; init; }
    internal required Func<TModel, TEntity> CreateEntityFromModel { private get; init; }
    internal required Action<TEntity, TModel> UpdateEntityFromModel { private get; init; }

    public virtual async Task<TModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        TModel? model = await Set
        .AsNoTracking()
        .Where(entity => entity.Id == id)
        .Select(Selector)
        .FirstOrDefaultAsync(cancellationToken);

        return model;
    }

    public virtual async Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        List<TModel> models = await Set
        .AsNoTracking()
        .Select(Selector)
        .ToListAsync(cancellationToken);

        return models;
    }

    public virtual async Task AddAsync(TModel model, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(CreateEntityFromModel(model), cancellationToken);

        await ApplicationContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Set.FirstAsync(entity => entity.Id == model.Id, cancellationToken);

        UpdateEntityFromModel(entity, model);

        await ApplicationContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await Set
        .Where(entity => entity.Id == id)
        .ExecuteDeleteAsync(cancellationToken);

        await ApplicationContext.SaveChangesAsync(cancellationToken);
    }
}