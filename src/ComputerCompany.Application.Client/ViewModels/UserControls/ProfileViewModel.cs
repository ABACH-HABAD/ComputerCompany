using System.Text;
using System.Windows.Input;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Application.Client.ViewModels.Dialogs;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class ProfileViewModel : BaseUserControlViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IScopeFactory _scopeFactory;

    private AccountModel _currentAccount = null!;
    private ReviewModel _currentReview = null!;

    private int _starCount = 5;
    private string _displayStars = ReviewModel.StarsToString(0);
    private string _accountName = string.Empty;
    private string _status = string.Empty;
    private string _login = string.Empty;
    private string _reviewText = string.Empty;
    private bool _isClient;

    private bool _isChangeingName;
    private bool _isChangeingLogin;
    private bool _isChangeingPassword;
    private bool _isDeletingReview;

    public AccountModel CurrentAccount
    {
        get => _currentAccount;
        private set
        {
            ChangeProperty(ref _currentAccount, value);
        }
    }

    public ReviewModel CurrentReview
    {
        get => _currentReview;
        set
        {
            ChangeProperty(ref _currentReview, value);
        }
    }

    public int StarCount
    {
        get => _starCount;
        set
        {
            ChangeProperty(ref _starCount, value);
            DisplayStars = ReviewModel.StarsToString(StarCount);
        }
    }

    public string DisplayStars
    {
        get => _displayStars;
        private set
        {
            ChangeProperty(ref _displayStars, value);
        }
    }

    public string AccountName
    {
        get => _accountName;
        private set
        {
            ChangeProperty(ref _accountName, value);
        }
    }

    public string Status
    {
        get => _status;
        private set
        {
            ChangeProperty(ref _status, value);
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

    public string ReviewText
    {
        get => _reviewText;
        set
        {
            ChangeProperty(ref _reviewText, value);
        }
    }

    public bool IsClient
    {
        get => _isClient;
        private set
        {
            ChangeProperty(ref _isClient, value);
        }
    }

    public bool IsChangeingName
    {
        get => _isChangeingName;
        set => ChangeProperty(ref _isChangeingName, value);
    }

    public bool IsChangeingLogin
    {
        get => _isChangeingLogin;
        set => ChangeProperty(ref _isChangeingLogin, value);
    }

    public bool IsChangeingPassword
    {
        get => _isChangeingPassword;
        set => ChangeProperty(ref _isChangeingPassword, value);
    }

    public bool IsDeletingReview
    {
        get => _isDeletingReview;
        set => ChangeProperty(ref _isDeletingReview, value);
    }

    public ICommand LoadAccountDataCommand { get; }
    public ICommand ChangeNameCommand { get; }
    public ICommand ChangeLoginCommand { get; }
    public ICommand ChangePasswordCommand { get; }
    public ICommand SendReviewCommand { get; }
    public ICommand DeleteReviewCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand DeleteAccountCommand { get; }

    public ProfileViewModel
    (
        INavigationService navigationService,
        IScopeFactory scopeFactory
    )
    {
        _navigationService = navigationService;
        _scopeFactory = scopeFactory;

        LoadAccountDataCommand = new AsyncRelayCommand(LoadAccountDataAsync);
        ChangeNameCommand = new AsyncRelayCommand(ChangeDisplayNameAsync, CanChangeName);
        ChangeLoginCommand = new AsyncRelayCommand(ChangeLoginAsync, CanChangeLogin);
        ChangePasswordCommand = new AsyncRelayCommand(ChangePasswordAsync, CanChangePassword);
        SendReviewCommand = new AsyncRelayCommand(SendReviewAsync);
        DeleteReviewCommand = new AsyncRelayCommand(DeleteReviewAsync, CanDeleteReview);
        LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        DeleteAccountCommand = new AsyncRelayCommand(DeleteAccountAsync);

        LoadAccountDataCommand.Execute(null);
    }

    private async Task LoadAccountDataAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();
        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();

        DataResult<AccountModel> account = await accountService.GetByIdAsync(default);
        if (!account.IsSuccess) messageService.ShowErrorMessage(account.Message);

        CurrentAccount = account.Data;
        AccountName = account.Data.Name;
        Status = roleCache.DisplayRoleNames[account.Data.Role.Name];
        Login = account.Data.Login;
        IsClient = account.Data.Role.Id == roleCache.UserRole.Id;

        MainViewModel mainViewModel = scope.ServiceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.AccountName = AccountName;
        mainViewModel.DisplayRole = Status;

        DataResult<ReviewModel> review = await reviewService.GetByAccountAsync(account.Data.Id);
        if (review.IsSuccess)
        {
            CurrentReview = review.Data;
            ReviewText = review.Data.Message;
            StarCount = review.Data.Stars;
        }
        else StarCount = 5;
    }

    private async Task ChangeDisplayNameAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        DataResult<AccountModel> dialogResult = await dialogService.ShowUpdateDialogAsync<AccountModel, ChangeNameViewModel>(CurrentAccount, "Смена имени");

        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        Result serviceResult = await accountService.UpdateAsync(dialogResult.Data);

        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }

        LoadAccountDataCommand.Execute(null);
    }

    private async Task ChangeLoginAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        DataResult<AccountModel> dialogResult = await dialogService.ShowUpdateDialogAsync<AccountModel, ChangeLoginViewModel>(CurrentAccount, "Смена логина");

        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        Result serviceResult = await accountService.UpdateAsync(dialogResult.Data);

        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }

        LoadAccountDataCommand.Execute(null);
    }

    private async Task ChangePasswordAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        Result dialogResult = await dialogService.ShowDataDialogAsync<AccountModel, PasswordResetViewModel>(title: "Сброс пароля");

        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        if (dialogResult is DataResult<object> dataResult)
        {
            (string oldPassword, string newPassword) = dataResult.Data as (string, string)? ?? throw new Exception("Ошибка при получении пароля из диалогового окна");

            Result serviceResult = await accountService.ResetPasswordAsync(CurrentAccount.Id, oldPassword, newPassword);
            if (!serviceResult.IsSuccess) messageService.ShowErrorMessage(serviceResult.Message);
            else messageService.ShowInformationMessage("Ваш пароль был успешно изменён!");
        }
        else throw new Exception("Ошибка при получении пароля из диалогового окна");


        LoadAccountDataCommand.Execute(null);
    }

    private async Task LogoutAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

        Result result = await sessionService.LogoutAsync(default);
        if (!result.IsSuccess)
        {
            messageService.ShowErrorMessage(result.Message);
            return;
        }

        IWindowService windowService = scope.ServiceProvider.GetRequiredService<IWindowService>();
        windowService.CloseWindow<MainViewModel>();
    }

    private async Task SendReviewAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

        if (ReviewText == null || ReviewText == string.Empty)
        {
            messageService.ShowErrorMessage("Напишите отзыв");
            return;
        }

        if (StarCount < 1 || StarCount > 5)
        {
            messageService.ShowErrorMessage("Некорректное количество звёзд");
            return;
        }

        ReviewModel review = CurrentReview == null ?
        new ReviewModel(default, CurrentAccount, ReviewText, StarCount) :
        CurrentReview with { Message = ReviewText, Stars = StarCount };

        IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

        Result result = await reviewService.SendReviewAsync(review);

        if (result.IsSuccess) messageService.ShowInformationMessage("Отзыв отправлен!");
        else messageService.ShowErrorMessage(result.Message);

        LoadAccountDataCommand.Execute(null);
    }

    public async Task DeleteReviewAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

        if (CurrentReview == null || CurrentReview.Id == Guid.Empty)
        {
            messageService.ShowErrorMessage("Вы ещё не оставили отзыв", "Отзыв не существует");
            return;
        }

        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        Result dialogResult = await dialogService.ShowDeleteDialogAsync("Удаление отзыва", "Вы уверены, что хотите удалить отзыв?");
        if (dialogResult.IsSuccess)
        {
            IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();
            Result result = await reviewService.DeleteAsync(CurrentReview);

            if (result.IsSuccess)
            {
                messageService.ShowInformationMessage("Отзыв успешно удалён");

                ReviewText = string.Empty;
                StarCount = 5;
            }
            else
            {
                messageService.ShowErrorMessage(result.Message);
                return;
            }
        }
        else
        {
            messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        LoadAccountDataCommand.Execute(null);
    }

    private async Task DeleteAccountAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        Result dialogResult = await dialogService.ShowDeleteDialogAsync("Удаление аккаунта", "Вы точно уверены что хотите навсегда удалить свой аккаунт?\nЭто действие невозможно будет отменить!");
        if (dialogResult.IsSuccess)
        {
            IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            Result result = await accountService.DeleteAsync(id: default);

            if (result.IsSuccess)
            {
                messageService.ShowInformationMessage("Ваш аккаунт был успешно удалён");

                IWindowService windowService = scope.ServiceProvider.GetRequiredService<IWindowService>();
                windowService.CloseWindow<MainViewModel>();
            }
            else
            {
                messageService.ShowErrorMessage(result.Message);
            }
        }
    }

    private bool CanChangeName() => !_isChangeingName;
    private bool CanChangeLogin() => !_isChangeingLogin;
    private bool CanChangePassword() => !_isChangeingPassword;

    private bool CanDeleteReview() => !_isDeletingReview;
}