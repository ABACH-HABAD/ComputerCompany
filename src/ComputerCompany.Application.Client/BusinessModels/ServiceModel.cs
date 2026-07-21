using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.BusinessModels;

public record ServiceModel(Guid Id, string Name, string Description, double Price) : BaseProductModel(Id, Name, Description, Price);