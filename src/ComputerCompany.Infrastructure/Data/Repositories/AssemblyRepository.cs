using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class AssemblyRepository : BaseRepository, IAssemblyRepository
{
    private readonly CoreRepository<AssemblyModel, AssemblyEntity> _coreRepository;

    public AssemblyRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Assemblies);
        coreRepositoryBuilder.AddSelector(AssemblySelector.ToAssemblyModel);
        coreRepositoryBuilder.AddCreateFunc<AssemblyEntity, AssemblyModel>(model =>
        {
            return new AssemblyEntity
            {
                CpuId = model.Cpu.Id,
                GpuId = model.Gpu.Id,
                MemoryId = model.Memory.Id,
                MotherboardId = model.Motherboard.Id,
                StorageId = model.Storage.Id,
                FrameId = model.Frame.Id,
                PowerUnitId = model.PowerUnit.Id,
                UsedMemoryCount = model.UsedMemoryCount,
                AccountId = model.Account.Id,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<AssemblyEntity, AssemblyModel>((entity, model) =>
        {
            entity.CpuId = model.Cpu.Id;
            entity.GpuId = model.Gpu.Id;
            entity.MemoryId = model.Memory.Id;
            entity.MotherboardId = model.Motherboard.Id;
            entity.StorageId = model.Storage.Id;
            entity.FrameId = model.Frame.Id;
            entity.PowerUnitId = model.PowerUnit.Id;
            entity.UsedMemoryCount = model.UsedMemoryCount;
            entity.AccountId = model.Account.Id;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<AssemblyModel, AssemblyEntity>();
    }

    public Task<AssemblyModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<AssemblyModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(AssemblyModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(AssemblyModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}