using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class ChangeLoginViewModel : BaseDataDialogViewModel<AccountModel>
{
    private readonly IEmailValidator _emailVaidator;
    private readonly IMessageService _messageService;
    private AccountModel _account = null!;

    private string _login = string.Empty;

    public string ParametrName { get; } = "Логин:";

    public string ParametrValue
    {
        get => _login;
        set
        {
            ChangeProperty(ref _login, value);
        }
    }

    public override AccountModel Data => _account with { Login = ParametrValue };


    public ChangeLoginViewModel(IMessageService message, IEmailValidator emailValidator)
    {
        _emailVaidator = emailValidator;
        _messageService = message;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(Declare);
    }

    public override void SetData(AccountModel data)
    {
        _account = data;
        ParametrValue = data.Login;
    }

    private void Accept()
    {
        if (ParametrValue == string.Empty || ParametrValue == null)
        {
            _messageService.ShowErrorMessage("Введите логин");
            return;
        }

        Result validationResult = _emailVaidator.Validate(ParametrValue);
        if (!validationResult.IsSuccess)
        {
            _messageService.ShowErrorMessage("Логин должен быть настоящей электронной почтой");
            return;
        }

        DialogResult = true;
    }

    private void Declare()
    {
        DialogResult = false;
    }
}