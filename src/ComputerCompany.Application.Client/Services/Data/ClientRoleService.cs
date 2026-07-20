using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientRoleService(IApiClientService apiClient) : BaseClientService<RoleModel>("role", null!, apiClient), IRoleService
{
    public override Task<Result> AddAsync(RoleModel data, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Клиент не имеет права редактировать роли в системе");
    }

    public override Task<Result> UpdateAsync(RoleModel data, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Клиент не имеет права редактировать роли в системе");
    }

    public override Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("Клиент не имеет права редактировать роли в системе");
    }
}