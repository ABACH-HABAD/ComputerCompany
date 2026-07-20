using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;

namespace ComputerCompany.Application.Services.Validation;

public class RoleValidator(IRoleCache roleCache, IValidator<INameable> nameValidator) : IValidator<RoleModel>
{
    public Result Validate(RoleModel data)
    {
        if (data == null) return Result.Fail("Роль " + ValidationMessages.Absent);

        Result nameValidationResult = nameValidator.Validate(data);
        if (!nameValidationResult.IsSuccess) return nameValidationResult;

        if (!roleCache.Contains(data)) return Result.Fail("Такой роли не существует");

        return Result.Success();
    }
}