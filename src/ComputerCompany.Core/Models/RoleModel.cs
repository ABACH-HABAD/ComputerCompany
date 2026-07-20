using ComputerCompany.Core.Models.Abstractions;
using System.Collections.Immutable;

namespace ComputerCompany.Core.Models;

public record RoleModel(Guid Id, string Name) : BaseModel(Id), INameable
{
    private static readonly ImmutableDictionary<string, string> _displayRoleNames = ImmutableDictionary.CreateRange
    (
        [
            KeyValuePair.Create("User", "Пользователь"),
            KeyValuePair.Create("Employee", "Сотрудник"),
            KeyValuePair.Create("Cashier", "Кассир"),
            KeyValuePair.Create("Manager", "Менеджер"),
            KeyValuePair.Create("Admin", "Администратор")
        ]
    );

    public override string ToString()
    {
        return _displayRoleNames[Name];
    }
}