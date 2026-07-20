using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class GpuSelector
{
    internal static Expression<Func<GpuEntity, GpuModel>> ToGpuModel = gpu =>
    new GpuModel(gpu.Id, gpu.Name, gpu.Description, gpu.Price, gpu.Count, gpu.Model, gpu.ModelCore, gpu.VideoMemory);
}