using System.ComponentModel;
using ComputerCompany.Application.Client.ViewModels;

namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface INavigationService
{
    public BaseUserControlViewModel CurrentViewModel { get; }
    public object CurrentUserControl { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void NavigateTo<TViewModel>() where TViewModel : BaseUserControlViewModel;
    public void NavigateTo<TViewModel>(INavigationOwnerViewModel navigationOwnerViewModel) where TViewModel : BaseUserControlViewModel;
}