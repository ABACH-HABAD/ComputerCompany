using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Data;

public abstract class BaseDataService<TModel, TRepository>(IValidator<TModel> validator, TRepository repository, string modelName = "данные") : IDataService<TModel>
where TModel : BaseModel
where TRepository : IRepository<TModel>
{
    protected readonly IValidator<TModel> _validator = validator;
    protected readonly TRepository _repository = repository;

    protected readonly string _modelName = modelName;

    public virtual async Task<DataResult<TModel>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            TModel? data = await _repository.GetByIdAsync(id, cancellationToken);

            if (data == null) return DataResult<TModel>.Fail($"Не удалось получить {_modelName}");
            else return DataResult<TModel>.Success(data);
        }
        catch (Exception ex)
        {
            return DataResult<TModel>.Fail(ex.Message);
        }
    }

    public virtual async Task<DataResult<List<TModel>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            List<TModel> data = await _repository.GetAllAsync(cancellationToken);

            return DataResult<List<TModel>>.Success(data);
        }
        catch (Exception ex)
        {
            return DataResult<List<TModel>>.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> AddAsync(TModel data, CancellationToken cancellationToken = default)
    {
        try
        {
            Result validationResult = _validator.Validate(data);
            if (!validationResult.IsSuccess) return validationResult;

            await _repository.AddAsync(data, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> UpdateAsync(TModel data, CancellationToken cancellationToken = default)
    {
        try
        {
            Result validationResult = _validator.Validate(data);
            if (!validationResult.IsSuccess) return validationResult;

            await _repository.UpdateAsync(data, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _repository.DeleteAsync(id, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}