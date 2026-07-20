using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class MemorySelector
{
    internal static Expression<Func<MemoryEntity, MemoryModel>> ToMemoryModel = memory =>
    new MemoryModel(memory.Id, memory.Name, memory.Description, memory.Price, memory.Count, memory.Model, memory.Type, memory.Size, memory.Frequency);
}