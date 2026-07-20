using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Validation.AbstractValidators;

public class CountValidator : IValidator<ICountable>
{
    public Result Validate(ICountable data)
    {
        return NumberValidator.ValidateNumber(data.Count, "Количество");
    }
}