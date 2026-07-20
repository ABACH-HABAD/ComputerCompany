using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public class MotherboardValidator
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
)
: BaseComponentValidator<MotherboardModel>(countValidator, nameValidator, descriptionValidator, priceValidator, modelValidator)
{
    public override Result Validate(MotherboardModel data)
    {
        _componentName = "Материнская плата";
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = StringValidator.ValidateString(data.Socket, "Сокет", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = StringValidator.ValidateString(data.Chipset, "Чипсет", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        return base.Validate(data);
    }
}