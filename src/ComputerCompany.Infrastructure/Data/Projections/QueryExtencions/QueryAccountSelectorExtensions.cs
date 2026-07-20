using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryAccountSelectorExtensions
{
    internal static IQueryable<AccountModel> ToAccountModel(this IQueryable<AccountEntity> query)
    {
        return query.Select(AccountSelector.ToAccountModel);
    }
}