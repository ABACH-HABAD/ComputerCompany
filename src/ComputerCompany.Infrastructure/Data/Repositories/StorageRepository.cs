using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class StorageRepository : BaseRepository, IStorageRepository
{
    private readonly CoreRepository<StorageModel, StorageEntity> _coreRepository;

    public StorageRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Storages);
        coreRepositoryBuilder.AddSelector(StorageSelector.ToStorageModel);
        coreRepositoryBuilder.AddCreateFunc<StorageEntity, StorageModel>(model =>
        {
            return new StorageEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                FormFactor = model.FormFactor,
                Size = model.Size,
                Type = model.Type,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<StorageEntity, StorageModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.FormFactor = model.FormFactor;
            entity.Size = model.Size;
            entity.Type = model.Type;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<StorageModel, StorageEntity>();
    }

    public Task<StorageModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<StorageModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(StorageModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(StorageModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}