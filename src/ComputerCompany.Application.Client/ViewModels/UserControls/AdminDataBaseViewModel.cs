using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Client.ViewModels.Events;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Windows.Input;


namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class AdminDataBaseViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;

    private event EventHandler<LoadCollectionEventArgs<AccountModel>> LoadAccountCollection;

    public ObservableCollection<AccountModel> Accounts { get; } = [];

    private bool _isPasswordReseting;
    private bool _isUpdatingAccount;
    private bool _isDeletingAccount;
    private bool _isCreateingAccount;

    public bool IsPasswordReseting
    {
        get => _isPasswordReseting;
        set
        {
            ChangeProperty(ref _isPasswordReseting, value);
        }
    }

    public bool IsUpdatingAccount
    {
        get => _isUpdatingAccount;
        set
        {
            ChangeProperty(ref _isUpdatingAccount, value);
        }
    }

    public bool IsDeletingAccount
    {
        get => _isDeletingAccount;
        set
        {
            ChangeProperty(ref _isDeletingAccount, value);
        }
    }

    public bool IsCreateingAccount
    {
        get => _isCreateingAccount;
        set
        {
            ChangeProperty(ref _isCreateingAccount, value);
        }
    }

    public ICommand LoadAccountsCommand { get; }
    public ICommand ResetPasswordCommand { get; }
    public ICommand UpdateAccountCommand { get; }
    public ICommand DeleteAccountCommand { get; }
    public ICommand CreateAccountCommand { get; }

    public AdminDataBaseViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        LoadAccountsCommand = new AsyncRelayCommand(LoadAccountsAsync);
        ResetPasswordCommand = new AsyncRelayCommand<AccountModel>(PasswordResetAsync, CanResetPassword!);
        UpdateAccountCommand = new AsyncRelayCommand<AccountModel>(UpdateAccountAsync, CanUpdateAccount!);
        DeleteAccountCommand = new AsyncRelayCommand<AccountModel>(DeleteAccountAsync, CanDeleteAccount!);
        CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync, CanCreateAccount!);

        LoadAccountCollection += (s, e) =>
        {
            if (s != this) return;
            Accounts.Clear();
            foreach (AccountModel account in e.Collection)
            {
                Accounts.Add(account);
            }
        };

        LoadAccountsCommand.Execute(null);
    }

    private async Task LoadAccountsAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        DataResult<List<AccountModel>> serviceResult = await accountService.GetAllAsync();
        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }

        LoadAccountCollection.Invoke(this, new LoadCollectionEventArgs<AccountModel>(serviceResult.Data));
    }

    private async Task PasswordResetAsync(AccountModel account)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();

        if (account.Role.Id == roleCache.AdminRole.Id)
        {
            messageService.ShowErrorMessage("Невозможно сбросить пароль другому администратору");
            return;
        }

        IsPasswordReseting = true;
        Result dialogResult = await dialogService.ShowDataDialogAsync<AccountModel, PasswordResetViewModel>(title: "Сброс пароля", parametrs: [PasswordResetViewModel.AsAdmin]);
        if (dialogResult.IsSuccess)
        {
            if (dialogResult is DataResult<object> dataResult)
            {
                (string oldPassword, string newPassword) = dataResult.Data as (string, string)? ?? throw new Exception("Ошибка при получении пароля из диалогового окна");

                Result serviceResult = await accountService.ResetPasswordAsync(account.Id, oldPassword, newPassword, forceChange: true);
                if (!serviceResult.IsSuccess) messageService.ShowErrorMessage(serviceResult.Message);
                else messageService.ShowInformationMessage("Пароль успешно изменён");
            }
            else throw new Exception("Ошибка при получении пароля из диалогового окна");
        }
        else
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
        }

        LoadAccountsCommand.Execute(null);
        IsPasswordReseting = false;
    }

    private async Task UpdateAccountAsync(AccountModel account)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();

        IsUpdatingAccount = true;
        DataResult<AccountModel> dialogResult = await dialogService.ShowUpdateDialogAsync<AccountModel, EditAccountViewModel>
        (
            account,
            "Редактирование аккаунта",
            [roleCache.Roles]
        );
        if (dialogResult.IsSuccess)
        {
            Result serviceResult = await accountService.UpdateAsync(dialogResult.Data);
            if (!serviceResult.IsSuccess) messageService.ShowErrorMessage(serviceResult.Message);
        }
        else
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
        }
        LoadAccountsCommand.Execute(null);
        IsUpdatingAccount = false;
    }

    private async Task DeleteAccountAsync(AccountModel account)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();

        if (account.Role.Id == roleCache.AdminRole.Id)
        {
            messageService.ShowErrorMessage("Невозможно удалить аккаунт другого администратора");
            return;
        }

        IsDeletingAccount = true;
        bool answer = messageService.ShowQuestionMessage
        (
             $"Вы уверны что хотите удалть аккаунт {account.Name}?",
             "Удаление аккаунта"
        );
        if (answer)
        {
            Result result = await accountService.DeleteAsync(account.Id);

            if (result.IsSuccess) messageService.ShowInformationMessage("Аккаунт успешно удалён");
            else messageService.ShowErrorMessage(result.Message);
        }
        LoadAccountsCommand.Execute(null);
        IsDeletingAccount = false;
    }

    private async Task CreateAccountAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        IsCreateingAccount = true;
        Result dialogResult = await dialogService.ShowDataDialogAsync<AccountModel, CreateAccountViewModel>(title: "Создание нового аккаунта");
        if (dialogResult.IsSuccess)
        {
            if (dialogResult is DataResult<object> dataResult)
            {
                (string login, string password, string repeatPassword) = dataResult.Data as (string, string, string)?
                ?? throw new Exception("Ошибка при получении пароля из диалогового окна");

                Result serviceResult = await accountService.RegistrateAsync(login, password, repeatPassword, saveLoginData: false);
                if (!serviceResult.IsSuccess) messageService.ShowErrorMessage(serviceResult.Message);
            }
            else throw new Exception("Ошибка при получении пароля из диалогового окна");
        }
        else
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
        }
        LoadAccountsCommand.Execute(null);
        IsCreateingAccount = false;
    }

    private bool CanResetPassword(AccountModel accountModel) => !_isPasswordReseting;
    private bool CanUpdateAccount(AccountModel account) => !_isUpdatingAccount;
    private bool CanDeleteAccount(AccountModel account) => !_isDeletingAccount;
    private bool CanCreateAccount() => !_isCreateingAccount;
}