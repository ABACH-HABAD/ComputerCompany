using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.ViewModels.Events;

public class DisplayReviewEventArgs(ReviewModel reviewModel) : EventArgs
{
    public ReviewModel ReviewModel { get; init; } = reviewModel;
}