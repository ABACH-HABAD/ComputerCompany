using ComputerCompany.Application.Client.ViewModels;

namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface INavigationOwnerViewModel
{
    public INavigationService NavigationService { get; }
    public object CurrentUserControl => NavigationService.CurrentUserControl;
    public BaseUserControlViewModel CurrentViewModel => NavigationService.CurrentViewModel;
}