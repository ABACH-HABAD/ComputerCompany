using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryRoleSelectorExtensions
{
    internal static IQueryable<RoleModel> ToRoleModel(this IQueryable<RoleEntity> query)
    {
        return query.Select(RoleSelector.ToRoleModel);
    }
}