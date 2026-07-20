using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class RoleSelector
{
    internal static Expression<Func<RoleEntity, RoleModel>> ToRoleModel = role =>
    new RoleModel(role.Id, role.Name);
}