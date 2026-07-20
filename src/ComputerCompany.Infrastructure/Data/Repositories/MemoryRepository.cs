using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class MemoryRepository : BaseRepository, IMemoryRepository
{
    private readonly CoreRepository<MemoryModel, MemoryEntity> _coreRepository;

    public MemoryRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Memories);
        coreRepositoryBuilder.AddSelector(MemorySelector.ToMemoryModel);
        coreRepositoryBuilder.AddCreateFunc<MemoryEntity, MemoryModel>(model =>
        {
            return new MemoryEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                Type = model.Type,
                Size = model.Size,
                Frequency = model.Frequency,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<MemoryEntity, MemoryModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.Type = model.Type;
            entity.Size = model.Size;
            entity.Frequency = model.Frequency;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<MemoryModel, MemoryEntity>();
    }

    public Task<MemoryModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<MemoryModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(MemoryModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(MemoryModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}