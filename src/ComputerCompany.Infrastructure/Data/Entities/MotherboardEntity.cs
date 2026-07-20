using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class MotherboardEntity : BaseComponent, IModelable
{
    public string Model { get; set; } = string.Empty;
    public string Socket { get; set; } = string.Empty;
    public string Chipset { get; set; } = string.Empty;
}