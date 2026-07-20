using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;
using Microsoft.EntityFrameworkCore;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class SessionRepository : BaseRepository, ISessionRepository
{
    private readonly CoreRepository<SessionModel, SessionEntity> _coreRepository;

    public SessionRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Sessions);
        coreRepositoryBuilder.AddSelector(SessionSelector.ToSessionModel);
        coreRepositoryBuilder.AddCreateFunc<SessionEntity, SessionModel>(model =>
        {
            return new SessionEntity
            {
                AccountId = model.Account.Id,
                Refresh = model.Refresh,
                Ip = model.Ip,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<SessionEntity, SessionModel>((entity, model) =>
        {
            entity.AccountId = model.Account.Id;
            entity.Refresh = model.Refresh;
            entity.Ip = model.Ip;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<SessionModel, SessionEntity>();
    }

    public Task<SessionModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<SessionModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(SessionModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(SessionModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);

    public async Task<SessionModel?> GetByTokenAsync(string refresh, string ip, CancellationToken cancellationToken = default)
    {
        SessionModel? sessionModel = await _applicationContext.Sessions
        .AsNoTracking()
        .Where(session => session.Refresh == refresh && session.Ip == ip)
        .ToSessionModel()
        .FirstOrDefaultAsync(cancellationToken);

        return sessionModel;
    }

    public async Task<SessionModel?> GetByAccountAsync(Guid accountId, string ip, CancellationToken cancellationToken = default)
    {
        SessionModel? sessionModel = await _applicationContext.Sessions
        .AsNoTracking()
        .Where(session => session.Account.Id == accountId && session.Ip == ip)
        .ToSessionModel()
        .FirstOrDefaultAsync(cancellationToken);

        return sessionModel;
    }
}