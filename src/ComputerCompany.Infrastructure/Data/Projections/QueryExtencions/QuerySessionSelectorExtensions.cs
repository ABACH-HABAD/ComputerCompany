using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QuerySessionSelectorExtensions
{
    internal static IQueryable<SessionModel> ToSessionModel(this IQueryable<SessionEntity> query)
    {
        return query.Select(SessionSelector.ToSessionModel);
    }
}