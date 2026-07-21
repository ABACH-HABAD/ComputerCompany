using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Services.Api;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Client.ViewModels;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Services.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace ComputerCompany.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    protected readonly IHost _host;

    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        HostBuilder builder = new();

        string basePath = AppContext.BaseDirectory;
        string appsettingsPath = Path.Combine(basePath, "appsettings.Development.json");

        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile(appsettingsPath, optional: false, reloadOnChange: true);

        IConfigurationRoot Configuration = configurationBuilder.Build();

        IConfigurationSection apiSettings = Configuration.GetSection("ApiSettings") ?? throw new Exception("ApiSettings не найден");

        string apiUrl = apiSettings["ApiUrl"] ?? throw new Exception($"{nameof(apiUrl)} не настроен");
        int timeout = int.Parse(apiSettings["TimeoutSeconds"] ?? throw new Exception($"{nameof(timeout)} не настроен"));

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(new ApiSettings(apiUrl, timeout));

            services.AddBusinessLogic();
            services.AddViewModels();
            services.AddWindows();
            services.AddUserControls();
        });

        _host = builder.Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();
        Services = _host.Services;

        IScopeFactory scopeFactory = Services.GetRequiredService<IScopeFactory>();

        //Загружаем кэш ролей один раз, чтобы потом не дёргать базу 
        using (IServiceScope scope = scopeFactory.CreateScope())
        {
            IRoleCache roleCache = scope.ServiceProvider.GetRequiredService<IRoleCache>();
        }

        //Пытаемся авторизоваться по уже имеющимуся токену
        bool loadAuthWindow = true;
        bool serverIsNotConnected = false;
        using (IServiceScope scope = scopeFactory.CreateScope())
        {
            ISessionService sessionService = scope.ServiceProvider.GetRequiredService<ISessionService>();

            LoginResult logingResult = await sessionService.RefreshTokensAsync(default!);
            if (logingResult.IsSuccess) loadAuthWindow = false;

            //Если сервер не отвечает
            if (!logingResult.IsSuccess && logingResult.Message == ApiResult.NotConnectedMessage) serverIsNotConnected = true;
        }

        using (IServiceScope scope = scopeFactory.CreateScope())
        {
            IWindowService windowService = scope.ServiceProvider.GetRequiredService<IWindowService>();
            
            //Если ещё не авторизированны, открываем страниуц логина и пароля
            if (loadAuthWindow)
            {
                WelcomeViewModel welcomeViewModel = scope.ServiceProvider.GetRequiredService<WelcomeViewModel>();

                welcomeViewModel.TryAutoConnect = serverIsNotConnected; //Пытаемся восстановить подключение если его нету

                welcomeViewModel.Closed += (s, e) => //Подписываемся на событие, чтобы перейти со страницы авторизации на главную страницу
                {
                    using IServiceScope clouseScope = scopeFactory.CreateScope();

                    MainViewModel mainView = clouseScope.ServiceProvider.GetRequiredService<MainViewModel>();
                    mainView.Closed += async (s, e) => //Подписываемся на событие, чтобы выключить приложение
                    {
                        await _host.StopAsync();
                        Shutdown();
                    };

                    windowService.ShowWindow<MainViewModel>();
                };

                windowService.ShowWindow<WelcomeViewModel>();
            }
            //Если мы уже авторизированы, сразу открываем главную страницу
            else
            {
                MainViewModel mainView = scope.ServiceProvider.GetRequiredService<MainViewModel>();
                mainView.Closed += async (s, e) =>  //Подписываемся на событие, чтобы выключить приложение
                {
                    await _host.StopAsync();
                    Shutdown();
                };

                windowService.ShowWindow<MainViewModel>();
            }
        }

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        base.OnExit(e);
    }
}