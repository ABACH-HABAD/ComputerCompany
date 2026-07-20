using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class HomeViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private readonly INavigationService _navigationService;

    public ICommand NavigateToBuildUserControl { get; }
    public ICommand NavigateToUpgradeUserControl { get; }
    public ICommand NavigateToRepairUserControl { get; }
    public ICommand NavigateToComponentUserControl { get; }

    public HomeViewModel(INavigationService navigationService, IScopeFactory scopeFactory)
    {
        _navigationService = navigationService;
        _scopeFactory = scopeFactory;

        NavigateToBuildUserControl = new RelayCommand(NavigateTo<BuildViewModel>);
        NavigateToUpgradeUserControl = new RelayCommand(NavigateTo<UpgradeViewModel>);
        NavigateToRepairUserControl = new RelayCommand(() => NotSupported("На данный момент опция починки компьютеров недоступна.\nПриносим свои извинения за доставленные неудобаства."));
        NavigateToComponentUserControl = new RelayCommand(NavigateTo<ComponentsViewModel>);
    }

    public void NavigateTo<TPage>() where TPage : BaseUserControlViewModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        MainViewModel mainViewModel = scope.ServiceProvider.GetRequiredService<MainViewModel>();
        _navigationService.NavigateTo<TPage>(mainViewModel);
    }

    public void NotSupported(string message)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        messageService.ShowInformationMessage(message);
    }
}