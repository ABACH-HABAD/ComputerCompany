using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class PowerUnitRepository : BaseRepository, IPowerUnitRepository
{
    private readonly CoreRepository<PowerUnitModel, PowerUnitEntity> _coreRepository;

    public PowerUnitRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.PowerUnits);
        coreRepositoryBuilder.AddSelector(PowerUnitSelector.ToPowerUnitModel);
        coreRepositoryBuilder.AddCreateFunc<PowerUnitEntity, PowerUnitModel>(model =>
        {
            return new PowerUnitEntity
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Count = model.Count,
                Model = model.Model,
                Power = model.Power,
                Certification = model.Certification,
                FormFactor = model.FormFactor,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<PowerUnitEntity, PowerUnitModel>((entity, model) =>
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price;
            entity.Count = model.Count;
            entity.Model = model.Model;
            entity.Power = model.Power;
            entity.Certification = model.Certification;
            entity.FormFactor = model.FormFactor;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<PowerUnitModel, PowerUnitEntity>();
    }

    public Task<PowerUnitModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<PowerUnitModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(PowerUnitModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(PowerUnitModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}