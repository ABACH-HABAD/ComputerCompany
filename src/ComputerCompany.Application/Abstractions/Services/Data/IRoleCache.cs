using System.Collections.Immutable;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Data;

public interface IRoleCache
{
    public ImmutableDictionary<string, string> DisplayRoleNames { get; }
    public RoleModel[] Roles { get; }

    public RoleModel UserRole { get; }
    public RoleModel EmployeeRole { get; }
    public RoleModel CashierRole { get; }
    public RoleModel ManagerRole { get; }
    public RoleModel AdminRole { get; }

    public bool Contains(RoleModel role);
}