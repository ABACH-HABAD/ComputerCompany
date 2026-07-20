using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryAssemblySelectorExtensions
{
    internal static IQueryable<AssemblyModel> ToAssemblyModel(this IQueryable<AssemblyEntity> query)
    {
        return query.Select(AssemblySelector.ToAssemblyModel);
    }
}