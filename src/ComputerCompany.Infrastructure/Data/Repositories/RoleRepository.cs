using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class RoleRepository : BaseRepository, IRoleRepository
{
    private readonly CoreRepository<RoleModel, RoleEntity> _coreRepository;

    public RoleRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Roles);
        coreRepositoryBuilder.AddSelector(RoleSelector.ToRoleModel);
        coreRepositoryBuilder.AddCreateFunc<RoleEntity, RoleModel>(model =>
        {
            return new RoleEntity
            {
                Name = model.Name,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<RoleEntity, RoleModel>((entity, model) =>
        {
            entity.Name = model.Name;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<RoleModel, RoleEntity>();
    }

    public Task<RoleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<RoleModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(RoleModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(RoleModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);
}