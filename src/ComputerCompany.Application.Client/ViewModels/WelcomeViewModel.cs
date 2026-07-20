using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;

namespace ComputerCompany.Application.Client.ViewModels;

public class WelcomeViewModel : BaseViewModel, IWindowViewModel, INavigationOwnerViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private readonly INavigationService _navigationService;

    public INavigationService NavigationService => _navigationService;

    public object CurrentUserControl => NavigationService.CurrentUserControl;
    public BaseUserControlViewModel CurrentViewModel => NavigationService.CurrentViewModel;

    public event EventHandler<EventArgs>? Closed;

    private bool _dialogResult;
    public bool DialogResult
    {
        get => _dialogResult;
        private set
        {
            _dialogResult = value;
            Close();
        }
    }

    public WelcomeViewModel(IScopeFactory scopeFactory, INavigationService navigationService)
    {
        _scopeFactory = scopeFactory;
        _navigationService = navigationService;
        _navigationService.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(CurrentUserControl));
        };

       // TryAutoLogin();
        _navigationService.NavigateTo<AuthorizationViewModel>();
    }

    public void Close()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}