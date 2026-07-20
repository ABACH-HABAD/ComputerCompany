using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryFrameSelectorExtensions
{
    internal static IQueryable<FrameModel> ToFrameModel(this IQueryable<FrameEntity> query)
    {
        return query.Select(FrameSelector.ToFrameModel);
    }
}