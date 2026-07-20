using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public class PowerUnitValidator
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
)
: BaseComponentValidator<PowerUnitModel>(countValidator, nameValidator, descriptionValidator, priceValidator, modelValidator)
{
    public override Result Validate(PowerUnitModel data)
    {
        _componentName = "Блок питания";
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = StringValidator.ValidateString(data.FormFactor, "Формфактор", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = StringValidator.ValidateString(data.Certification, "Сертификат", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = NumberValidator.ValidateNumber(data.Power, "Мощность");
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        return base.Validate(data);
    }
}