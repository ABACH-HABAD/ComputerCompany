using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record FrameModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string FormFactor
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable;