using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public abstract record BaseComponentModel(Guid Id, string Name, string Description, double Price, int Count, string Model)
: BaseProductModel(Id, Name, Description, Price), ICountable, IModelable;