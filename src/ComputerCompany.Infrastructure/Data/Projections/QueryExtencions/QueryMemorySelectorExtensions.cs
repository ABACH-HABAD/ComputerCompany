using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryMemorySelectorExtensions
{
    internal static IQueryable<MemoryModel> ToMemoryModel(this IQueryable<MemoryEntity> query)
    {
        return query.Select(MemorySelector.ToMemoryModel);
    }
}