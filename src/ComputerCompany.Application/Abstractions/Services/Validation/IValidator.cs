using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Abstractions.Services.Validation;

public interface IValidator<T>
{
    public Result Validate(T data);
}