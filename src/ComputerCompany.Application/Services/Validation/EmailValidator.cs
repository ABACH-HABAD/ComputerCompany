using System.Text.RegularExpressions;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Validation;

namespace ComputerCompany.Application.Abstractions.Validation;

public partial class EmailValidator : IEmailValidator
{
    public const string LoginPattern = @"\w*@\w*\.\w{2,4}$";

    public Result Validate(string data)
    {
        Result baseValidation = StringValidator.ValidateString(data, "логин", false);
        if (!baseValidation.IsSuccess) return baseValidation;

        if (!EmailRegex().IsMatch(data)) return Result.Fail("Логин должен быть насоящей электронной почтой\n");

        return Result.Success();
    }

    [GeneratedRegex(LoginPattern)]
    private static partial Regex EmailRegex();
}