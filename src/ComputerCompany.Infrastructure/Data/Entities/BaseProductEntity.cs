using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal abstract class BaseProductEntity : BaseEntity, INameable, IDescriptionable, IPriceable
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
}