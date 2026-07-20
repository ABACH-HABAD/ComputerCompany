using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class FrameEntity : BaseComponent, IModelable
{
    public string Model { get; set; } = string.Empty;
    public string FormFactor { get; set; } = string.Empty;
}