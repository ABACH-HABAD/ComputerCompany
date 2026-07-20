using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class StorageEntity : BaseComponent, IModelable
{
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string FormFactor { get; set; } = string.Empty;
    public int Size { get; set; }
}