using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Services.Validation;

internal static class NumberValidator
{
    internal static Result ValidateNumber(double number, string propertyName)
    {
        if (number <= 0) return Result.Fail(propertyName + ValidationMessages.CannotBeLessThanZero);

        return Result.Success();
    }

    internal static Result ValidateNumber(int number, string propertyName)
    {
        if (number <= 0) return Result.Fail(propertyName + ValidationMessages.CannotBeLessThanZero);

        return Result.Success();
    }
}