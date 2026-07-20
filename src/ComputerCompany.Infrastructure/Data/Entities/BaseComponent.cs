using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal abstract class BaseComponent : BaseProductEntity, ICountable
{
    public int Count { get; set; }

    public List<AssemblyEntity> Assemblies { get; set; } = [];
}