using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Services.Validation;

internal static class StringValidator
{
    internal static Result ValidateString(string data, string propertyName, bool canBeEmpty)
    {
        if (data == null) return Result.Fail(propertyName + ValidationMessages.Absent);

        if (data == string.Empty && !canBeEmpty) return Result.Fail(propertyName + ValidationMessages.CanNotBeEmpty);

        return Result.Success();
    }
}