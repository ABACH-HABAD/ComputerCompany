using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record PowerUnitModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string Certification,
    string FormFactor,
    int Power
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable; 