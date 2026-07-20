using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class PowerUnitEntity : BaseComponent, IModelable
{
    public string Model { get; set; } = string.Empty;
    public string FormFactor { get; set; } = string.Empty;
    public string Certification { get; set; } = string.Empty;
    public int Power { get; set; }
}