using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record GpuModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string ModelCore,
    int VideoMemory
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable;