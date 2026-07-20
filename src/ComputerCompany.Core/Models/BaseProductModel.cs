using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public abstract record BaseProductModel(Guid Id, string Name, string Description, double Price) : BaseModel(Id), INameable, IDescriptionable, IPriceable;