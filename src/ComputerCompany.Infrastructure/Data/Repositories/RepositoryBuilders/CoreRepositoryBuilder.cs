using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;

public class CoreRepositoryBuilder
{
    private ApplicationContext? _applicationContext;
    private object? _set;
    private object? _selector;
    private object? _createEntityFromModel;
    private object? _updateEntityFromModel;

    internal void AddContext(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    internal void AddSet<TEntity>(DbSet<TEntity> set)
    where TEntity : BaseEntity
    {
        _set = set;
    }

    internal void AddSelector<TEntity, TModel>(Expression<Func<TEntity, TModel>> selector)
    where TModel : BaseModel
    where TEntity : BaseEntity
    {
        _selector = selector;
    }

    internal void AddCreateFunc<TEntity, TModel>(Func<TModel, TEntity> func)
    where TModel : BaseModel
    where TEntity : BaseEntity
    {
        _createEntityFromModel = func;
    }

    internal void AddUpdateFunc<TEntity, TModel>(Action<TEntity, TModel> func)
    where TModel : BaseModel
    where TEntity : BaseEntity
    {
        _updateEntityFromModel = func;
    }

    internal CoreRepository<TModel, TEntity> BuildCoreRepository<TModel, TEntity>()
    where TModel : BaseModel
    where TEntity : BaseEntity
    {
        if (_applicationContext is null) throw new Exception($"Укажите {nameof(ApplicationContext)} с помощью {nameof(AddContext)}");
        if (_set is not DbSet<TEntity> set) throw new Exception($"Укажите {nameof(DbSet<TEntity>)} с помощью {nameof(AddSet)}");
        if (_selector is not Expression<Func<TEntity, TModel>> selector) throw new Exception($"Укажите {nameof(Expression<Func<TEntity, TModel>>)} с помощью {nameof(AddSelector)}");
        if (_createEntityFromModel is not Func<TModel, TEntity> createFunc) throw new Exception($"Укажите {nameof(Func<TModel, TEntity>)} с помощью {nameof(AddCreateFunc)}");
        if (_updateEntityFromModel is not Action<TEntity, TModel> updateFunc) throw new Exception($"Укажите {nameof(Action<TEntity, TModel>)} с помощью {nameof(AddUpdateFunc)}");

        CoreRepository<TModel, TEntity> buildedCoreRepository = new()
        {
            ApplicationContext = _applicationContext,
            Set = set,
            Selector = selector,
            CreateEntityFromModel = createFunc,
            UpdateEntityFromModel = updateFunc
        };

        return buildedCoreRepository;
    }
}