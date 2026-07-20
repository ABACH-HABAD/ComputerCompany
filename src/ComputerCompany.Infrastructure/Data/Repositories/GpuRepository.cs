using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class GpuRepository : BaseRepository, IGpuRepository
{
    private readonly CoreRepository<GpuModel, GpuEntity> _coreRepository;

    public GpuRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Gpus);
        coreRepositoryBuilder.AddSelector(GpuSelector.ToGpuModel);
        coreRepositoryBuilder.AddCreateFunc<GpuEntity, GpuModel>(model =>
        {
            return new GpuEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                ModelCore = model.ModelCore,
                VideoMemory = model.VideoMemory,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<GpuEntity, GpuModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.ModelCore = model.ModelCore;
            entity.VideoMemory = model.VideoMemory;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<GpuModel, GpuEntity>();
    }

    public Task<GpuModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<GpuModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(GpuModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(GpuModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}