using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryPowerUnitSelectorExtensions
{
    internal static IQueryable<PowerUnitModel> ToPowerUnitModel(this IQueryable<PowerUnitEntity> query)
    {
        return query.Select(PowerUnitSelector.ToPowerUnitModel);
    }
}