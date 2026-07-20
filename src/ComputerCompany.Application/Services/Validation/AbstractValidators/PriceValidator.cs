using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Validation.AbstractValidators;

public class PriceValidator : IValidator<IPriceable>
{
    public Result Validate(IPriceable data)
    {
       return NumberValidator.ValidateNumber(data.Price, "Цена");
    }
}