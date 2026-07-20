using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation.ComponentValidators;

public abstract class BaseComponentValidator<T>
(
    IValidator<ICountable> countValidator,
    IValidator<INameable> nameValidator,
    IValidator<IDescriptionable> descriptionValidator,
    IValidator<IPriceable> priceValidator,
    IValidator<IModelable> modelValidator
) : IValidator<T> where T : BaseComponentModel
{
    protected readonly LinkedList<string> _errors = [];
    protected string _componentName = string.Empty;

    public virtual Result Validate(T data)
    {
        if (data == null) return Result.Fail(_componentName + ValidationMessages.Absent);

        Result result;

        result = countValidator.Validate(data);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = nameValidator.Validate(data);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = descriptionValidator.Validate(data);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = priceValidator.Validate(data);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        result = modelValidator.Validate(data);
        if (!result.IsSuccess) _errors.AddLast(result.Message);

        if (_errors.Count > 0) return Result.Fail(result.Message);
        else return Result.Success();
    }
}