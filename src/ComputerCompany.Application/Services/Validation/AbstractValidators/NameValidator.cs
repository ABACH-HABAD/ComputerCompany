using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Validation.AbstractValidators;

public class NameValidator : IValidator<INameable>
{
    public Result Validate(INameable data)
    {
        return StringValidator.ValidateString(data.Name, propertyName: "Имя", canBeEmpty: false);
    }
}