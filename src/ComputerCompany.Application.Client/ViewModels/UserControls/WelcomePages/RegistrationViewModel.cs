using System.Windows.Input;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;

public class RegistrationViewModel : BaseUserControlViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IScopeFactory _scopeFactory;

    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _repeatPassword = string.Empty;

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

    public string RepeatPassword
    {
        get => _repeatPassword;
        set
        {
            ChangeProperty(ref _repeatPassword, value);
        }
    }

    public ICommand RegistrationCommand { get; }
    public ICommand NavigateToAuthorizationCommand { get; }

    public RegistrationViewModel
    (
        INavigationService navigationService,
        IScopeFactory scopeFactory
    )
    {
        _navigationService = navigationService;
        _scopeFactory = scopeFactory;

        RegistrationCommand = new AsyncRelayCommand(RegistrationAsync);
        NavigateToAuthorizationCommand = new RelayCommand(AuthorizationToRegistration);
    }

    public async Task RegistrationAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

        if (Login == string.Empty)
        {
            messageService.ShowErrorMessage("Введите логин");
            return;
        }

        if (Password != RepeatPassword)
        {
            messageService.ShowErrorMessage("Пароли не совпадают");
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

        if (Password != RepeatPassword)
        {
            messageService.ShowErrorMessage("Пароли не совпадают");
            return;
        }

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        LoginResult loginResult = await accountService.RegistrateAsync(Login, Password, RepeatPassword);
        if (!loginResult.IsSuccess)
        {
            messageService.ShowErrorMessage(loginResult.Message);
            return;
        }

        IWindowService windowService = scope.ServiceProvider.GetRequiredService<IWindowService>();

        windowService.CloseWindow<WelcomeViewModel>();
        windowService.ShowWindow<MainViewModel>();
    }

    private void AuthorizationToRegistration()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        WelcomeViewModel welcomeViewModel = scope.ServiceProvider.GetRequiredService<WelcomeViewModel>();
        _navigationService.NavigateTo<AuthorizationViewModel>(welcomeViewModel);
    }

    public override void OnNavigatedFrom()
    {
        Login = string.Empty;
        Password = string.Empty;
        RepeatPassword = string.Empty;
    }

    public override void OnNavigatedTo()
    {
        Login = string.Empty;
        Password = string.Empty;
        RepeatPassword = string.Empty;
    }
}