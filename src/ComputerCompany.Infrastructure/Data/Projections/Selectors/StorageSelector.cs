using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class StorageSelector
{
    internal static Expression<Func<StorageEntity, StorageModel>> ToStorageModel = storage =>
    new StorageModel(storage.Id, storage.Name, storage.Description, storage.Price, storage.Count, storage.Model, storage.FormFactor, storage.Type, storage.Size);
}