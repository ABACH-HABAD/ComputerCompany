using System.Text.RegularExpressions;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Validation;

namespace ComputerCompany.Application.Abstractions.Validation;

public partial class PasswordValidator : IPasswordValidator
{
    public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";

    public Result Validate(string data)
    {
        Result baseValidation = StringValidator.ValidateString(data, "пароль", false);
        if (!baseValidation.IsSuccess) return baseValidation;

        if (!EmailRegex().IsMatch(data)) return Result.Fail("Пароль должен быть от 8 до 15 символов и содеражить как минимум 1 сточный и 1 прописной символ\n");

        return Result.Success();
    }

    [GeneratedRegex(PasswordPattern)]
    private static partial Regex EmailRegex();
}