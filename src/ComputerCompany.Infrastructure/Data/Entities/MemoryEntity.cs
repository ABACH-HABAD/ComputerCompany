using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class MemoryEntity : BaseComponent, IModelable
{
    public string Model { get; set; } = string.Empty;
    public int Size { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Frequency { get; set;  }
}