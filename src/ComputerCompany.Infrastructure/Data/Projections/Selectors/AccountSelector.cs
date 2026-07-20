using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class AccountSelector
{
    public static Expression<Func<AccountEntity, AccountModel>> ToAccountModel = account =>
    new AccountModel(account.Id, account.Login, account.Name, new RoleModel(account.Role.Id, account.Role.Name));
}