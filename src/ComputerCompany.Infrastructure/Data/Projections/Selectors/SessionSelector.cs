using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class SessionSelector
{
    public static Expression<Func<SessionEntity, SessionModel>> ToSessionModel = session =>
    new SessionModel
    (
        session.Id, 
        session.Refresh,
        session.Account != null ? new AccountModel
        (
            session.Account.Id, 
            session.Account.Login, 
            session.Account.Name,
            new RoleModel(session.Account.Role.Id, session.Account.Role.Name)
        ) : null!,
        session.Ip
    );
}