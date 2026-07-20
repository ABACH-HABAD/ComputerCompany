using System.Windows.Input;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class PasswordResetViewModel : BaseDialogViewModel, IDataRequireableViewModel, IDataRequireableViewModel<bool>
{
    public const bool AsAdmin = true;
    private readonly IMessageService _messageService;
    private readonly IPasswordValidator _passwordValidator;

    private string _oldPassword = string.Empty;
    private string _newPassword = string.Empty;

    private bool _isAdmin = false;

    public string OldPassword
    {
        get => _oldPassword;
        set
        {
            ChangeProperty(ref _oldPassword, value);
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            ChangeProperty(ref _newPassword, value);
        }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set => ChangeProperty(ref _isAdmin, value);
    }

    public override object? ResultData { get => (OldPassword, NewPassword); }

    public PasswordResetViewModel(IMessageService messageService, IPasswordValidator passwordValidator)
    {
        _passwordValidator = passwordValidator;
        _messageService = messageService;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(Cancel);
    }
    public void LoadData(params object[] data)
    {
        foreach (object item in data)
        {
            try
            {
                bool foundedBoolean = (bool)item;
                LoadData(foundedBoolean);
            }
            catch
            {
                continue;
            }
        }
    }

    public void LoadData(bool data)
    {
        IsAdmin = data;
    }

    private void Accept()
    {
        if (!IsAdmin && (OldPassword == null || OldPassword == string.Empty))
        {
            _messageService.ShowErrorMessage("Введите старый пароль");
            return;
        }

        if (NewPassword == null || NewPassword == string.Empty)
        {
            _messageService.ShowErrorMessage("Введите новый пароль");
            return;
        }

        Result validationResult = _passwordValidator.Validate(NewPassword);
        if (!validationResult.IsSuccess)
        {
            _messageService.ShowErrorMessage(validationResult.Message);
            return;
        }

        DialogResult = true;
    }

    private void Cancel()
    {
        DialogResult = false;
    }
}