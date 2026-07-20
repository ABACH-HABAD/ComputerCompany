using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryMotherboardSelectorExtensions
{
    internal static IQueryable<MotherboardModel> ToMotherboardModel(this IQueryable<MotherboardEntity> query)
    {
        return query.Select(MotherboardSelector.ToMotherboardModel);
    }
}