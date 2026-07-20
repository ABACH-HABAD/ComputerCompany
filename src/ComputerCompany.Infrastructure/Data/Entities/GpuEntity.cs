using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class GpuEntity : BaseComponent, IModelable
{
    public string ModelCore { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int VideoMemory { get; set; }
}