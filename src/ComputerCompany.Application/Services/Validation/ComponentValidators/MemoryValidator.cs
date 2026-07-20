using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public class MemoryValidator
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
)
: BaseComponentValidator<MemoryModel>(countValidator, nameValidator, descriptionValidator, priceValidator, modelValidator)
{
    public override Result Validate(MemoryModel data)
    {
        _componentName = "Оперативная память";
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = StringValidator.ValidateString(data.Type, "Тип", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = NumberValidator.ValidateNumber(data.Frequency, "Частота");
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = NumberValidator.ValidateNumber(data.Size, "Объём");
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        return base.Validate(data);
    }
}