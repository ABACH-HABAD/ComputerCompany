using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using ComputerCompany.Infrastructure.Data.Projections.Selectors;

namespace ComputerCompany.Infrastructure.Data.Projections.QueryExtencions;

internal static class QueryReviewSelectorExtensions
{
    public static IQueryable<ReviewModel> ToReviewModel(this IQueryable<ReviewEntity> query)
    {
        return query.Select(ReviewSelector.ToReviewModel);
    }
}