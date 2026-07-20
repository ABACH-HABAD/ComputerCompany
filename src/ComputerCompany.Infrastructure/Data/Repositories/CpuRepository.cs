using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class CpuRepository : BaseRepository, ICpuRepository
{
    private readonly CoreRepository<CpuModel, CpuEntity> _coreRepository;

    public CpuRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Cpus);
        coreRepositoryBuilder.AddSelector(CpuSelector.ToCpuModel);
        coreRepositoryBuilder.AddCreateFunc<CpuEntity, CpuModel>(model =>
        {
            return new CpuEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                Socket = model.Socket,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<CpuEntity, CpuModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.Socket = model.Socket;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<CpuModel, CpuEntity>();
    }

    public Task<CpuModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<CpuModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(CpuModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(CpuModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}