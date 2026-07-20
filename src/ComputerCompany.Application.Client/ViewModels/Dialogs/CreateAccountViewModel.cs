using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class CreateAccountViewModel : BaseDialogViewModel
{
    private readonly IMessageService _messageService;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IEmailValidator _emailValidator;

    private readonly Random _random = new();

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

    public override object? ResultData => (Login, Password, RepeatPassword);

    public CreateAccountViewModel(IMessageService messageService, IEmailValidator emailValidator, IPasswordValidator passwordValidator)
    {
        _messageService = messageService;
        _emailValidator = emailValidator;
        _passwordValidator = passwordValidator;

        DenyCommand = new RelayCommand(() => DialogResult = false);
        AcceptCommand = new RelayCommand(Accept);
    }

    public void Accept()
    {
        if (Login == string.Empty)
        {
            _messageService.ShowErrorMessage("Введите логин");
            return;
        }

        Result validationResult;
        validationResult = _emailValidator.Validate(Login);
        if (!validationResult.IsSuccess)
        {
            _messageService.ShowErrorMessage(validationResult.Message);
            return;
        }

        if (Password != RepeatPassword)
        {
            _messageService.ShowErrorMessage("Пароли не совпадают");
            return;
        }

        if (Password == string.Empty)
        {
            _messageService.ShowErrorMessage("Введите пароль");
            return;
        }

        validationResult = _passwordValidator.Validate(Password);
        if (!validationResult.IsSuccess)
        {
            _messageService.ShowErrorMessage(validationResult.Message);
            return;
        }


        DialogResult = true;
    }
}
