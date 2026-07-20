using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record StorageModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string FormFactor,
    string Type,
    int Size
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable;