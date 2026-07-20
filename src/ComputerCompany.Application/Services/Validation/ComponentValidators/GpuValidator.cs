using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public class GpuValidator
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
)
: BaseComponentValidator<GpuModel>(countValidator, nameValidator, descriptionValidator, priceValidator, modelValidator)
{
    public override Result Validate(GpuModel data)
    {
        _componentName = "Видеокарта";
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = StringValidator.ValidateString(data.ModelCore, "Видеочип", false);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = NumberValidator.ValidateNumber(data.VideoMemory, "Видеопамять");
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        return base.Validate(data);
    }
}