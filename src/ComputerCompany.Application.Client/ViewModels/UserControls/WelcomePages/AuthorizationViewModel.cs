using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;

public class AuthorizationViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private readonly INavigationService _navigationService;

    private string _login = string.Empty;
    private string _password = string.Empty;

    public string Login
    {
        get => _login;
        set
        {
            ChangeProperty(ref _login, value);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            ChangeProperty(ref _password, value);
        }
    }

    public ICommand AuthorizationCommand { get; }
    public ICommand NavigateToRegistrationCommand { get; }

    public AuthorizationViewModel
    (
        INavigationService navigationService,
        IScopeFactory scopeFactory
    )
    {
        _navigationService = navigationService;
        _scopeFactory = scopeFactory;

        AuthorizationCommand = new AsyncRelayCommand(AuthorizationAsync);
        NavigateToRegistrationCommand = new RelayCommand(NavigateToRegistration);
    }

    private async Task AuthorizationAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

        if (Login == string.Empty)
        {
            messageService.ShowErrorMessage("Введите логин");
            return;
        }

        if (Password == string.Empty)
        {
            messageService.ShowErrorMessage("Введите пароль");
            return;
        }

        IEmailValidator emailValidator = scope.ServiceProvider.GetRequiredService<IEmailValidator>();
        IPasswordValidator passwordValidator = scope.ServiceProvider.GetRequiredService<IPasswordValidator>();

        Result validationResult;

        validationResult = emailValidator.Validate(Login);
        if (!validationResult.IsSuccess)
        {
            messageService.ShowErrorMessage(validationResult.Message);
            return;
        }

        validationResult = passwordValidator.Validate(Password);
        if (!validationResult.IsSuccess)
        {
            messageService.ShowErrorMessage(validationResult.Message);
            return;
        }

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        LoginResult loginResult = await accountService.LoginAsync(Login, Password);
        if (!loginResult.IsSuccess)
        {
            messageService.ShowErrorMessage(loginResult.Message);
            return;
        }

        IWindowService windowService = scope.ServiceProvider.GetRequiredService<IWindowService>();

        windowService.CloseWindow<WelcomeViewModel>();
        windowService.ShowWindow<MainViewModel>();
    }

    private void NavigateToRegistration()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        WelcomeViewModel welcomeViewModel = scope.ServiceProvider.GetRequiredService<WelcomeViewModel>();
        _navigationService.NavigateTo<RegistrationViewModel>(welcomeViewModel);
    }

    public override void OnNavigatedFrom()
    {
        Login = string.Empty;
        Password = string.Empty;
    }

    public override void OnNavigatedTo()
    {
        Login = string.Empty;
        Password = string.Empty;
    }
}