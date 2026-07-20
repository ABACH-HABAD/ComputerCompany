using ComputerCompany.Core.Models;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Abstractions.Services.Data;

public interface IDataService<T> where T : BaseModel
{
    public Task<DataResult<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<DataResult<List<T>>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Result> AddAsync(T data, CancellationToken cancellationToken = default);
    public Task<Result> UpdateAsync(T data, CancellationToken cancellationToken = default);
    public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Result> DeleteAsync(T data, CancellationToken cancellationToken = default) => DeleteAsync(data.Id, cancellationToken);
}