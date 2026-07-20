using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class RoleService
(
    IRoleRepository repository
) : BaseDataService<RoleModel, IRoleRepository>(null!, repository, "роль"),
IRoleService
{
    public override Task<Result> AddAsync(RoleModel data, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Таблицу Роли нельзя редактировать!");
    }

    public override Task<Result> UpdateAsync(RoleModel data, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Таблицу Роли нельзя редактировать!");
    }

    public override Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Таблицу Роли нельзя редактировать!");
    }
}