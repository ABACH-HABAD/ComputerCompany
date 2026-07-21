using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerCompany.Application.Client.ViewModels;

public class WelcomeViewModel : BaseViewModel, IWindowViewModel, INavigationOwnerViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private readonly INavigationService _navigationService;

    private bool _tryAutoConnect;
    private bool _dialogResult;

    private event EventHandler<EventArgs> ConnectionWasRestored;
    public event EventHandler<EventArgs>? Closed;

    public INavigationService NavigationService => _navigationService;

    public object CurrentUserControl => NavigationService.CurrentUserControl;
    public BaseUserControlViewModel CurrentViewModel => NavigationService.CurrentViewModel;

    public bool DialogResult
    {
        get => _dialogResult;
        private set
        {
            _dialogResult = value;
            Close();
        }
    }

    public bool TryAutoConnect
    {
        private get => _tryAutoConnect;
        set
        {
            _tryAutoConnect = value;
            if (_tryAutoConnect)
            {
                Task.Run(BackgroundTryAutoConnect).ConfigureAwait(false);
            }
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

        ConnectionWasRestored += (s, e) =>
        {
            Close();
        };
    }

    public void Close()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private async void BackgroundTryAutoConnect()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

        while (TryAutoConnect)
        {
            await Task.Delay(1000);
            LoginResult logingResult = await sessionService.RefreshTokensAsync(default!);

            //если подключится удалось
            if (logingResult.Message != ApiResult.NotConnectedMessage)
            {
                //и войти в аккаунт тоже
                if (logingResult.IsSuccess)
                {
                    //то закрваем окно входа
                    ConnectionWasRestored?.Invoke(this, new EventArgs());
                }
                //если же сессия устарела, то завершаем попытки войти в аккаунт
                //и спокойно даём пользователю ввести логин и пароль
                else break;
            }
        }
    }
}