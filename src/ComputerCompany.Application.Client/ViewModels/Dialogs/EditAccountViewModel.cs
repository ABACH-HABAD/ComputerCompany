using System.Collections.ObjectModel;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Abstractions.Services.Data;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class EditAccountViewModel : BaseDataDialogViewModel<AccountModel>, IDataRequireableViewModel, IDataRequireableViewModel<IEnumerable<RoleModel>>
{
    private readonly IMessageService _messageService;
    private readonly IEmailValidator _emailValidator;
    private readonly IRoleCache _roleCache;

    private Guid _accountId = default;

    private string _login = string.Empty;
    private string _name = string.Empty;

    private RoleModel _oldRole = null!;
    private RoleModel _selectedRole = null!;

    public ObservableCollection<RoleModel> Roles { get; } = [];
    public RoleModel SelectedRole
    {
        get => _selectedRole;
        set
        {
            ChangeProperty(ref _selectedRole, value);
        }
    }

    public string Login
    {
        get => _login;
        set
        {
            ChangeProperty(ref _login, value);
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            ChangeProperty(ref _name, value);
        }
    }

    public override AccountModel Data => new
    (
        _accountId,
        Login,
        Name,
        SelectedRole
    );

    public EditAccountViewModel(IMessageService messageService, IEmailValidator emailValidator, IRoleCache roleCache)
    {
        _messageService = messageService;
        _emailValidator = emailValidator;
        _roleCache = roleCache;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(Cancel);
    }

    public void LoadData(params object[] data)
    {
        foreach (object item in data)
        {
            if (item is IEnumerable<RoleModel> roles) LoadData(roles);
        }
    }

    public void LoadData(IEnumerable<RoleModel> data)
    {
        Roles.Clear();
        foreach (RoleModel item in data)
        {
            Roles.Add(item);
        }
    }

    public override void SetData(AccountModel data)
    {
        _accountId = data.Id;
        Name = data.Name;
        Login = data.Login;

        foreach (RoleModel role in Roles)
        {
            if (role.Id == data.Role.Id)
            {
                _oldRole = role;
                SelectedRole = role;
                break;
            }
        }
    }

    private void Accept()
    {
        if (Login == string.Empty)
        {
            _messageService.ShowErrorMessage("Введите логин");
            return;
        }

        Result validationResult = _emailValidator.Validate(Login);
        if (!validationResult.IsSuccess)
        {
            _messageService.ShowErrorMessage(validationResult.Message);
            return;
        }

        if (Name == string.Empty)
        {
            _messageService.ShowErrorMessage("Введите имя");
            return;
        }

        if (SelectedRole == null)
        {
            _messageService.ShowErrorMessage("Выберете роль");
            return;
        }
        else
        {
            if (_oldRole.Id == _roleCache.AdminRole.Id && SelectedRole.Id != _roleCache.AdminRole.Id)
            {
                _messageService.ShowErrorMessage("Невозможно понизить в должности администратора");
                return;
            }
        }

        DialogResult = true;
    }

    private void Cancel()
    {
        DialogResult = false;
    }
}