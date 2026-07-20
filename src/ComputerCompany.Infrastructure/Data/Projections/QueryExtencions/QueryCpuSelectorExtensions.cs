using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryCpuSelectorExtensions
{
    internal static IQueryable<CpuModel> ToCpuModel(this IQueryable<CpuEntity> query)
    {
        return query.Select(CpuSelector.ToCpuModel);
    }
}