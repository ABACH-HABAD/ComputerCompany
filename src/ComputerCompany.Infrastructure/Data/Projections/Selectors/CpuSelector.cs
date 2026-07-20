using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class CpuSelector
{
    internal static Expression<Func<CpuEntity, CpuModel>> ToCpuModel = cpu =>
    new CpuModel(cpu.Id, cpu.Name, cpu.Description, cpu.Price, cpu.Count, cpu.Model, cpu.Socket);
}