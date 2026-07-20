using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal abstract class BaseEntity : IIdable
{
    public Guid Id { get; set; }
}