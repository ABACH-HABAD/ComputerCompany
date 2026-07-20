using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record MemoryModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string Type,
    int Size,
    int Frequency
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable;  