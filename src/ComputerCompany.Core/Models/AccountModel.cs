using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record AccountModel(Guid Id, string Login, string Name, RoleModel Role) : BaseModel(Id), INameable;