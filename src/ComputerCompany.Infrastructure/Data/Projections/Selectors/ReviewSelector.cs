using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class ReviewSelector
{
    internal static Expression<Func<ReviewEntity, ReviewModel>> ToReviewModel = review =>
    new ReviewModel
    (
        review.Id,
        new AccountModel
        (
            review.Sender.Id, 
            review.Sender.Login,
            review.Sender.Name, 
            new RoleModel(review.Sender.Role.Id, review.Sender.Role.Name)
        ),
        review.Message,
        review.Stars
    );
}