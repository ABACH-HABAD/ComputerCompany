using Microsoft.EntityFrameworkCore;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

namespace ComputerCompany.Infrastructure.Data.Repositories;

public class AccountRepository : BaseRepository, IAccountRepository
{
    private readonly CoreRepository<AccountModel, AccountEntity> _coreRepository;

    public AccountRepository(ApplicationContext applicationContext, CoreRepositoryBuilder coreRepositoryBuilder) : base(applicationContext)
    {
        coreRepositoryBuilder.AddContext(applicationContext);
        coreRepositoryBuilder.AddSet(applicationContext.Accounts);
        coreRepositoryBuilder.AddSelector(AccountSelector.ToAccountModel);
        coreRepositoryBuilder.AddCreateFunc<AccountEntity, AccountModel>(model =>
        {
            return new AccountEntity
            {
                Login = model.Login,
                Name = model.Name,
                RoleId = model.Role.Id,
            };
        });
        coreRepositoryBuilder.AddUpdateFunc<AccountEntity, AccountModel>((entity, model) =>
        {
            entity.Login = model.Login;
            entity.Name = model.Name;
            entity.RoleId = model.Role.Id;
        });

        _coreRepository = coreRepositoryBuilder.BuildCoreRepository<AccountModel, AccountEntity>();
    }

    public Task<AccountModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.GetByIdAsync(id, cancellationToken);
    public Task<List<AccountModel>> GetAllAsync(CancellationToken cancellationToken = default) => _coreRepository.GetAllAsync(cancellationToken);

    public Task AddAsync(AccountModel model, CancellationToken cancellationToken = default) => _coreRepository.AddAsync(model, cancellationToken);
    public Task UpdateAsync(AccountModel model, CancellationToken cancellationToken = default) => _coreRepository.UpdateAsync(model, cancellationToken);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) => _coreRepository.DeleteAsync(id, cancellationToken);

    public async Task<AccountModel?> GetByLoginAndPasswordAsync(string login, string password, CancellationToken cancellationToken = default)
    {
        AccountEntity? accountEntity = await _applicationContext.Accounts
        .AsNoTracking()
        .Include(account => account.Role)
        .FirstOrDefaultAsync(account => account.Login == login, cancellationToken);

        if (accountEntity == null) return null;
        else if (accountEntity.Password != password) return null;

        return new AccountModel(accountEntity.Id, accountEntity.Login, accountEntity.Name, new RoleModel(accountEntity.Role.Id, accountEntity.Role.Name));
    }

    public async Task<AccountModel?> GetByLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        AccountModel? account = await _applicationContext.Accounts
        .AsNoTracking()
        .Where(account => account.Login == login)
        .ToAccountModel()
        .FirstOrDefaultAsync(cancellationToken);

        return account;
    }

    public async Task AddAsync(AccountModel model, string password, CancellationToken cancellationToken = default)
    {
        AccountEntity account = new()
        {
            Login = model.Login,
            Name = model.Name,
            RoleId = model.Role.Id,
            Password = password,
        };

        await _applicationContext.Accounts.AddAsync(account, cancellationToken);

        await _applicationContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> CheckPasswordAsync(Guid userId, string password, CancellationToken cancellationToken = default)
    {
        AccountEntity? accountEntity = await _applicationContext.Accounts
        .AsNoTracking()
        .FirstOrDefaultAsync(account => account.Id == userId, cancellationToken);

        if (accountEntity == null) return false;
        else return accountEntity.Password == password;
    }

    public async Task ChangePasswordAsync(Guid id, string password, CancellationToken cancellationToken = default)
    {
        await _applicationContext.Accounts
        .Where(account => account.Id == id)
        .ExecuteUpdateAsync(setters =>
            setters.SetProperty(account => account.Password, password), cancellationToken);
    }
}