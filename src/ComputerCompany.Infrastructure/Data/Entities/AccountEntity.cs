using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class AccountEntity : BaseEntity, INameable
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public Guid RoleId { get; set; }

    public RoleEntity Role { get; set; } = null!;

    public List<AssemblyEntity> Assemblies { get; set; } = [];
    public List<SessionEntity> Sessions { get; set; } = [];
}