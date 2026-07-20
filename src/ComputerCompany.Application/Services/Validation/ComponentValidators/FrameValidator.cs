using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public class FrameValidator
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
)
: BaseComponentValidator<FrameModel>(countValidator, nameValidator, descriptionValidator, priceValidator, modelValidator)
{
    public override Result Validate(FrameModel data)
    {
        _componentName = "Корпус";
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = StringValidator.ValidateString(data.FormFactor, "Формфактор", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        return base.Validate(data);
    }
}