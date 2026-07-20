using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record CpuModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string Socket
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable;