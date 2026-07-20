using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class MotherboardRepository : BaseRepository, IMotherboardRepository
{
    private readonly CoreRepository<MotherboardModel, MotherboardEntity> _coreRepository;

    public MotherboardRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Motherboards);
        coreRepositoryBuilder.AddSelector(MotherboardSelector.ToMotherboardModel);
        coreRepositoryBuilder.AddCreateFunc<MotherboardEntity, MotherboardModel>(model =>
        {
            return new MotherboardEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                Socket= model.Socket,
                Chipset = model.Chipset,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<MotherboardEntity, MotherboardModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.Socket = model.Socket;
            entity.Chipset = model.Chipset;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<MotherboardModel, MotherboardEntity>();
    }

    public Task<MotherboardModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<MotherboardModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(MotherboardModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(MotherboardModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}