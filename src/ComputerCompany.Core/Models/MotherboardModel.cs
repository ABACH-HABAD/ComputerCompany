using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public record MotherboardModel
(
    Guid Id,
    string Name,
    string Description,
    double Price,
    int Count,
    string Model,
    string Chipset,
    string Socket
) : BaseComponentModel(Id, Name, Description, Price, Count, Model), IModelable; 