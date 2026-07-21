using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Security;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Abstractions.Validation;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Application.Services.Security;
using ComputerCompany.Application.Services.Validation;
using ComputerCompany.Application.Services.Validation.AbstractValidators;
using ComputerCompany.Application.Services.Validation.ComponentValidators;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.Services.Api;
using ComputerCompany.Application.Client.Services.Data;
using ComputerCompany.Application.Client.Services.Security;
using ComputerCompany.Application.Client.Services.Shopping;
using ComputerCompany.Application.Client.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Client.ViewModels.UserControls;
using ComputerCompany.Application.Client.ViewModels.UserControls.WelcomePages;
using ComputerCompany.Presentation.Services;
using ComputerCompany.Presentation.Services.Dialog;
using ComputerCompany.Presentation.Services.Navigation;
using ComputerCompany.Presentation.Services.Token;
using ComputerCompany.Presentation.UserControls;
using ComputerCompany.Presentation.UserControls.WelcomeUserControls;
using ComputerCompany.Presentation.Windows;
using ComputerCompany.Presentation.Windows.Dialogs;

namespace ComputerCompany.Presentation;

public static class DependecyEnjectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IRoleCache, RoleCache>();

        services.AddScoped<IAccountService, ClientAccountService>();
        services.AddScoped<ISessionService, ClientSessionService>();
        services.AddScoped<IRoleService, ClientRoleService>();
        services.AddScoped<IReviewService, ClientReviewService>();
        services.AddScoped<IAssemblyService, ClientAssemblyService>();
        services.AddScoped<ICpuService, ClientCpuService>();
        services.AddScoped<IGpuService, ClientGpuService>();
        services.AddScoped<IMotherboardService, ClientMotherboardService>();
        services.AddScoped<IMemoryService, ClientMemoryService>();
        services.AddScoped<IStorageService, ClientStorageService>();
        services.AddScoped<IPowerUnitService, ClientPowerUnitService>();
        services.AddScoped<IFrameService, ClientFrameService>();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IEmailValidator, EmailValidator>();
        services.AddScoped<IPasswordValidator, PasswordValidator>();

        services.AddScoped<IValidator<INameable>, NameValidator>();
        services.AddScoped<IValidator<IDescriptionable>, DescriptionValidator>();
        services.AddScoped<IValidator<IModelable>, ModelValidator>();
        services.AddScoped<IValidator<IPriceable>, PriceValidator>();
        services.AddScoped<IValidator<ICountable>, CountValidator>();

        services.AddScoped<IValidator<AccountModel>, AccountValidator>();
        services.AddScoped<IValidator<SessionModel>, SessionValidator>();
        services.AddScoped<IValidator<RoleModel>, RoleValidator>();
        services.AddScoped<IValidator<ReviewModel>, ReviewValidator>();
        services.AddScoped<IValidator<AssemblyModel>, AssemblyValidator>();

        services.AddScoped<IValidator<CpuModel>, CpuValidator>();
        services.AddScoped<IValidator<GpuModel>, GpuValidator>();
        services.AddScoped<IValidator<MemoryModel>, MemoryValidator>();
        services.AddScoped<IValidator<StorageModel>, StorageValidator>();
        services.AddScoped<IValidator<MotherboardModel>, MotherboardValidator>();
        services.AddScoped<IValidator<PowerUnitModel>, PowerUnitValidator>();
        services.AddScoped<IValidator<FrameModel>, FrameValidator>();

        services.AddScoped<IAsseblyCheckerService, AssemblyCheckerService>();

        return services;
    }

    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IScopeFactory, WindowsScopeFactory>();
        services.AddSingleton<IViewModelsViewsMap, ViewModelsViewsMap>();
        services.AddSingleton<IMessageService, WindowMessageService>();

        services.AddScoped<IEncryption, EncryptionService>();
        services.AddScoped<IPasswordHasherService, PasswordHasher>();
        services.AddScoped<ITokenStorageService, TokenStorageService>();
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IDialogService, WindowDialogService>();
        services.AddScoped<IApiClientService, ApiService>();

        services.AddSingleton<IShoppingCartService, ShoppingCartService>();
        
        services.AddValidation();
        services.AddDataServices();

        return services;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<WelcomeViewModel>();
        services.AddSingleton<MainViewModel>();

        services.AddScoped<CardPayViewModel>();
        services.AddScoped<ChangeNameViewModel>();
        services.AddScoped<ChangeLoginViewModel>();
        services.AddScoped<CreateAccountViewModel>();
        services.AddScoped<ComponentDataBaseDialogViewModel<CpuModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<GpuModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<MotherboardModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<MemoryModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<StorageModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<PowerUnitModel>>();
        services.AddScoped<ComponentDataBaseDialogViewModel<FrameModel>>();
        services.AddScoped<DeleteAccountViewModel>();
        services.AddScoped<InformationViewModel>();
        services.AddScoped<PasswordResetViewModel>();
        services.AddScoped<QrViewModel>();
        services.AddScoped<EditAccountViewModel>();

        services.AddScoped<AuthorizationViewModel>();
        services.AddScoped<RegistrationViewModel>();

        services.AddScoped<HomeViewModel>();
        services.AddScoped<BuildViewModel>();
        services.AddScoped<UpgradeViewModel>();
        services.AddScoped<ProfileViewModel>();
        services.AddScoped<ShoppingCartViewModel>();
        services.AddScoped<ComponentsViewModel>();
        services.AddScoped<UpgradeViewModel>();

        services.AddScoped<ManagerDataBaseViewModel>();
        services.AddScoped<AdminDataBaseViewModel>();

        return services;
    }

    public static IServiceCollection AddWindows(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<WelcomeWindow>();

        services.AddScoped<AccountEditDialogWindow>();
        services.AddScoped<CardPay>();
        services.AddScoped<ChangeDisplayName>();
        services.AddScoped<CreateAccountWindow>();
        services.AddScoped<DataBaseAddDataWindow>();
        services.AddScoped<DeleteAccount>();
        services.AddScoped<InformationWindow>();
        services.AddScoped<PasswordResetDialog>();
        services.AddScoped<QRPay>();

        return services;
    }

    public static IServiceCollection AddUserControls(this IServiceCollection services)
    {
        services.AddScoped<AuthorizationUserControl>();
        services.AddScoped<RegistrationUserControl>();

        services.AddScoped<Profile>();
        services.AddScoped<ClientBuildUserControl>();
        services.AddScoped<ClientComponentUserControl>();
        services.AddScoped<ClientMainUserControl>();
        services.AddScoped<ClientShoppingCartUserControl>();
        services.AddScoped<ClientUpgradeUserControl>();
        services.AddScoped<ClientUpgradeUserControl>();
        services.AddScoped<ManagerDataBaseUserControl>();
        services.AddScoped<AdminDataBaseUserControl>();

        return services;
    }
}