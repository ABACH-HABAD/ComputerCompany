using System.Windows.Input;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.Services;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Client.ViewModels.Events;
using ComputerCompany.Application.Client.ViewModels.UserControls;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerCompany.Application.Client.ViewModels;

public class MainViewModel : BaseViewModel, IWindowViewModel, INavigationOwnerViewModel
{
    private const int ReviewUpdateDelay = 10;
    private readonly Queue<ReviewModel> _reviewsQueue = [];

    private readonly IScopeFactory _scopeFactory;
    private readonly INavigationService _navigationService;

    public INavigationService NavigationService => _navigationService;

    private string _accountName = string.Empty;
    private string _displayRole = string.Empty;
    private bool _isAdmin = false;
    private bool _isManager = false;

    private string _reviewAccountName = string.Empty;
    private string _reviewStars = string.Empty;
    private string _reviewText = string.Empty;

    private bool _isShowingInformation;

    public object CurrentUserControl => NavigationService.CurrentUserControl;
    public BaseUserControlViewModel CurrentViewModel => NavigationService.CurrentViewModel;

    public event EventHandler<EventArgs>? Closed;
    public event EventHandler<DisplayReviewEventArgs>? ChangeDisplayReview;

    public string AccountName
    {
        get => _accountName;
        set
        {
            ChangeProperty(ref _accountName, value);
        }
    }

    public string DisplayRole
    {
        get => _displayRole;
        set
        {
            ChangeProperty(ref _displayRole, value);
        }
    }

    public bool IsAdmin
    {
        get => _isAdmin;
        set
        {
            ChangeProperty(ref _isAdmin, value);
        }
    }

    public bool IsManager
    {
        get => _isManager;
        set
        {
            ChangeProperty(ref _isManager, value);
        }
    }

    public bool IsShowingInformation
    {
        get => _isShowingInformation;
        set
        {
            ChangeProperty(ref _isShowingInformation, value);
        }
    }

    public string ReviewAccountName
    {
        get => _reviewAccountName;
        set => ChangeProperty(ref _reviewAccountName, value);
    }
    public string ReviewStars
    {
        get => _reviewStars;
        set => ChangeProperty(ref _reviewStars, value);
    }
    public string ReviewText
    {
        get => _reviewText;
        set => ChangeProperty(ref _reviewText, value);
    }

    public ICommand NavigateToHomeUserControlCommand { get; }
    public ICommand NavigateToComponentsUserControlCommand { get; }
    public ICommand NavigateToUpgradeUserControlCommand { get; }
    public ICommand NavigateToOrderUserControlCommand { get; }
    public ICommand NavigateToBasketUserControlCommand { get; }
    public ICommand NavigateToDataBaseUserControlCommand { get; }
    public ICommand NavigateToEmployeesUserControlCommand { get; }
    public ICommand NavigateToProfileUserControlCommand { get; }

    public ICommand ShowMusicWindowCommand { get; }
    public ICommand ShowWeAreInSocialNetworksWindowCommand { get; }
    public ICommand ShowStoreLicenseWindowCommand { get; }
    public ICommand ShowAboutTheCompanyWindowCommand { get; }

    public ICommand LogoutCommand { get; }

    public ICommand UpdateProfileInfoCommand { get; }

    public MainViewModel(IScopeFactory scopeFactory, INavigationService navigationService)
    {
        _scopeFactory = scopeFactory;
        _navigationService = navigationService;
        _navigationService.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(CurrentViewModel));
            OnPropertyChanged(nameof(CurrentUserControl));
        };

        NavigateToHomeUserControlCommand = new RelayCommand(_navigationService.NavigateTo<HomeViewModel>);
        NavigateToComponentsUserControlCommand = new RelayCommand(_navigationService.NavigateTo<ComponentsViewModel>);
        NavigateToUpgradeUserControlCommand = new RelayCommand(_navigationService.NavigateTo<UpgradeViewModel>);
        NavigateToOrderUserControlCommand = new RelayCommand(_navigationService.NavigateTo<BuildViewModel>);
        NavigateToBasketUserControlCommand = new RelayCommand(_navigationService.NavigateTo<ShoppingCartViewModel>);
        NavigateToDataBaseUserControlCommand = new RelayCommand(_navigationService.NavigateTo<AdminDataBaseViewModel>);
        NavigateToEmployeesUserControlCommand = new RelayCommand(_navigationService.NavigateTo<ManagerDataBaseViewModel>);
        NavigateToProfileUserControlCommand = new RelayCommand(_navigationService.NavigateTo<ProfileViewModel>);

        ShowMusicWindowCommand = null!;
        ShowWeAreInSocialNetworksWindowCommand = new AsyncRelayCommand(ShowWeAreInSocialNetworksAsync, CanShowInformation);
        ShowStoreLicenseWindowCommand = new AsyncRelayCommand(ShowStoreLicenseAsync, CanShowInformation);
        ShowAboutTheCompanyWindowCommand = new AsyncRelayCommand(ShowAboutTheCompanyAsync, CanShowInformation);

        LogoutCommand = new AsyncRelayCommand(LogoutAsync);

        UpdateProfileInfoCommand = new AsyncRelayCommand(UpdateAccountInfoAsync);

        UpdateProfileInfoCommand.Execute(null);

        _navigationService.NavigateTo<HomeViewModel>();

        ChangeDisplayReview += (s, e) =>
        {
            ReviewModel review = e.ReviewModel;

            ReviewAccountName = e.ReviewModel.Sender.Name;
            ReviewStars = e.ReviewModel.DisplayStars;
            ReviewText = e.ReviewModel.Message;
        };
        Task.Run(AutoUpdateReviews);
    }

    public async Task UpdateReviewAsync()
    {
        if (_reviewsQueue.TryDequeue(out ReviewModel? review))
        {
            ChangeDisplayReview?.Invoke(this, new DisplayReviewEventArgs(review));
        }
        else
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            IReviewService reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();

            DataResult<List<ReviewModel>> result = await reviewService.GetRandomRangeAsync(10);
            if (result.IsSuccess)
            {
                foreach (ReviewModel model in result.Data)
                {
                    _reviewsQueue.Enqueue(model);
                }

                if (result.Data.Count > 0) await UpdateReviewAsync();
            }
        }
    }

    public async void AutoUpdateReviews()
    {
        while (true)
        {
            await UpdateReviewAsync();
            await Task.Delay(1000 * ReviewUpdateDelay);
        }
    }

    public async Task UpdateAccountInfoAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IAccountService accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();

        DataResult<AccountModel> serviceResult = await accountService.GetByIdAsync(default);
        if (!serviceResult.IsSuccess) return;

        AccountModel account = serviceResult.Data;
        AccountName = account.Name;

        IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();

        DisplayRole = roleCache.DisplayRoleNames[account.Role.Name];

        if (roleCache.AdminRole.Name == account.Role.Name)
        {
            IsManager = true;
            IsAdmin = true;
        }
        else if (roleCache.ManagerRole.Name == account.Role.Name)
        {
            IsManager = true;
            IsAdmin = false;
        }
        else
        {
            IsAdmin = false;
            IsManager = false;
        }
    }

    private async Task LogoutAsync()
    {
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

            await sessionService.LogoutAsync(default);
        }

        Close();
    }

    private async Task ShowInformationAsync(string title, string information)
    {
        IsShowingInformation = true;

        using IServiceScope scope = _scopeFactory.CreateScope();

        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();
        await dialogService.ShowInformationDialogAsync<InformationViewModel>(title, information);

        IsShowingInformation = false;
    }

    private async Task ShowWeAreInSocialNetworksAsync()
    {
        await ShowInformationAsync("Мы в социальных сетях", Constants.WeInSociatlNetworks);
    }

    private async Task ShowStoreLicenseAsync()
    {
        await ShowInformationAsync("Мы в социальных сетях", Constants.Licence);
    }

    private async Task ShowAboutTheCompanyAsync()
    {
        await ShowInformationAsync("Мы в социальных сетях", Constants.AboutCompany);
    }

    public void Close()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private bool CanShowInformation() => !_isShowingInformation;
}