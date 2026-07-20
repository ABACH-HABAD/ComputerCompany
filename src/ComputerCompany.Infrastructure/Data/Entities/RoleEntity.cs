using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Infrastructure.Data.Entities;

internal class RoleEntity : BaseEntity, INameable
{
    public string Name { get; set; } = string.Empty;

    public List<AccountEntity> Accounts { get; set; } = [];
}