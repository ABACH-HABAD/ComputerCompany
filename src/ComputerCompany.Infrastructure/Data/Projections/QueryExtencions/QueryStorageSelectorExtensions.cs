using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryStorageSelectorExtensions
{
    internal static IQueryable<StorageModel> ToStorageModel(this IQueryable<StorageEntity> query)
    {
        return query.Select(StorageSelector.ToStorageModel);
    }
}