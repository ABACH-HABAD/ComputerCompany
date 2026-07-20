using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Validation
{
    public class ReviewValidator : IValidator<ReviewModel>
    {
        public Result Validate(ReviewModel data)
        {
            if (data == null) return Result.Fail("Отзыв " + ValidationMessages.Absent);

            if (data.Sender == null) return Result.Fail("Отправитель " + ValidationMessages.Absent);

            if (data.Stars < 1 || data.Stars > 5) return Result.Fail("Количество звёзд может быть от 1 до 5");

            return Result.Success();
        }
    }
}