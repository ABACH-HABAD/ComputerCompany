using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Results;
using System.Collections.Immutable;

namespace ComputerCompany.Application.Services.Data;

public class RoleCache : IRoleCache
{
    private List<RoleModel> _roles = null!;

    public RoleModel[] Roles => [.. _roles];

    public RoleCache(IScopeFactory scopeFactory)
    {
        LoadRoles(scopeFactory);
    }

    public RoleModel UserRole => _roles[0];
    public RoleModel EmployeeRole => _roles[1];
    public RoleModel CashierRole => _roles[2];
    public RoleModel ManagerRole => _roles[3];
    public RoleModel AdminRole => _roles[4];

    public ImmutableDictionary<string, string> DisplayRoleNames { get; private set; } = null!;

    public bool Contains(RoleModel role)
    {
        foreach (RoleModel roleModel in _roles)
        {
            if (role.Id == roleModel.Id && role.Name == roleModel.Name) return true;
        }
        return false;
    }

    public async void LoadRoles(IScopeFactory scopeFactory)
    {
        using IServiceScope scope = scopeFactory.CreateScope();
        IRoleService roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();
        DataResult<List<RoleModel>> result = await roleService.GetAllAsync();
        if (!result.IsSuccess) throw new Exception(result.Message);
        _roles = result.Data;

        await Task.Delay(100);

        DisplayRoleNames = ImmutableDictionary.CreateRange
        (
            [
                KeyValuePair.Create(UserRole.Name, "Пользователь"),
                KeyValuePair.Create(EmployeeRole.Name, "Сотрудник"),
                KeyValuePair.Create(CashierRole.Name, "Кассир"),
                KeyValuePair.Create(ManagerRole.Name, "Менеджер"),
                KeyValuePair.Create(AdminRole.Name, "Администратор")
            ]
        );
    }
}