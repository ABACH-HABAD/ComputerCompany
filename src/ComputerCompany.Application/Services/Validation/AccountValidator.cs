using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Services.Validation;

public class AccountValidator(IValidator<INameable> nameValidator, IEmailValidator emailValidator, IValidator<RoleModel> roleValidator) : IValidator<AccountModel>
{
    public Result Validate(AccountModel data)
    {
        LinkedList<string> errors = [];

        if (data == null) return Result.Fail("Аккаунт" + ValidationMessages.Absent);

        Result emailValidationResult = emailValidator.Validate(data.Login);
        if (!emailValidationResult.IsSuccess) errors.AddLast(emailValidationResult.Message);

        Result nameValidationResult = nameValidator.Validate(data);
        if (!nameValidationResult.IsSuccess) errors.AddLast(nameValidationResult.Message);

        Result roleValidationResult = roleValidator.Validate(data.Role);
        if (!roleValidationResult.IsSuccess) errors.AddLast(roleValidationResult.Message);

        if (errors.Count > 0) return Result.Fail([.. errors]);
        else return Result.Success();
    }
}