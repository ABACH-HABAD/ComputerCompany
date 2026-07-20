using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Validation;

namespace ComputerCompany.Application.Services.Validation.AbstractValidators;

public class ModelValidator : IValidator<IModelable>
{
    public Result Validate(IModelable data)
    {
        return StringValidator.ValidateString(data.Model, propertyName: "Модель", canBeEmpty: false);
    }
}