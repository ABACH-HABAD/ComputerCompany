using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Core.Models;

public abstract record BaseModel(Guid Id) : IIdable;