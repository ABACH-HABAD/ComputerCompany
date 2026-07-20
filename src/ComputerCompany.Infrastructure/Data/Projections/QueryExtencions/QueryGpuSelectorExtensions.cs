using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryGpuSelectorExtensions
{
    internal static IQueryable<GpuModel> ToGpuModel(this IQueryable<GpuEntity> query)
    {
        return query.Select(GpuSelector.ToGpuModel);
    }
}