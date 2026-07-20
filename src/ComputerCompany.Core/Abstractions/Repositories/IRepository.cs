using ComputerCompany.Core.Models;

namespace ComputerCompany.Core.Abstractions.Repositories;

public interface IRepository<T> where T : BaseModel
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task AddAsync(T model, CancellationToken cancellationToken = default);
    public Task UpdateAsync(T model, CancellationToken cancellationToken = default);
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}