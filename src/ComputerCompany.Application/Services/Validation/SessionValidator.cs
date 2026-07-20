using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Validation;

public class SessionValidator : IValidator<SessionModel>
{
    public Result Validate(SessionModel data)
    {
        if (data == null) return Result.Fail("Сессия" + ValidationMessages.Absent);

        LinkedList<string> errors = [];

        Result result;

        result = StringValidator.ValidateString(data.Refresh, "Токен", false);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = StringValidator.ValidateString(data.Ip, "IP", false);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        if (data.Account == null) errors.AddLast("Аккаунт" + ValidationMessages.Absent);

        return Result.Success();
    }
}