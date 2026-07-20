using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Validation.AbstractValidators;

public class DescriptionValidator : IValidator<IDescriptionable>
{
    public Result Validate(IDescriptionable data)
    {
        return StringValidator.ValidateString(data.Description, propertyName: "Описание", canBeEmpty: true);
    }
}