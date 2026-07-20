using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;
using ComputerCompany.Core.Models.Abstractions;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Security;
using ComputerCompany.Application.Abstractions.Services.Token;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Abstractions.Validation;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Application.Services.Security;
using ComputerCompany.Application.Services.Token;
using ComputerCompany.Application.Services.Validation;
using ComputerCompany.Application.Services.Validation.AbstractValidators;
using ComputerCompany.Application.Services.Validation.ComponentValidators;
using ComputerCompany.Infrastructure.Data;
using ComputerCompany.Infrastructure.Data.Repositories;
using ComputerCompany.Infrastructure.Data.Repositories.RepositoryBuilders;
using ComputerCompany.WebApi.Services;

namespace ComputerCompany.WebApi;

public static class DependecyEnjectionExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string connection = builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Ненайдена строка подключения");
        services.AddSingleton(new DataBaseConnectionString(connection));
        services.AddDbContext<ApplicationContext>();

        services.AddScoped<CoreRepositoryBuilder>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IAssemblyRepository, AssemblyRepository>();
        services.AddScoped<ICpuRepository, CpuRepository>();
        services.AddScoped<IGpuRepository, GpuRepository>();
        services.AddScoped<IMotherboardRepository, MotherboardRepository>();
        services.AddScoped<IMemoryRepository, MemoryRepository>();
        services.AddScoped<IStorageRepository, StorageRepository>();
        services.AddScoped<IPowerUnitRepository, PowerUnitRepository>();
        services.AddScoped<IFrameRepository, FrameRepository>();

        return services;
    }

    public static IServiceCollection AddInfrastucture(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDataBase(builder);

        return services;
    }

    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<IRoleCache, RoleCache>();

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IAssemblyService, AssemblyService>();
        services.AddScoped<ICpuService, CpuService>();
        services.AddScoped<IGpuService, GpuService>();
        services.AddScoped<IMotherboardService, MotherboardService>();
        services.AddScoped<IMemoryService, MemoryService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<IPowerUnitService, PowerUnitService>();
        services.AddScoped<IFrameService, FrameService>();

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

        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddJwtAuthentication(builder.Configuration);

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        services.AddScoped<IPasswordHasherService, PasswordHasher>();

        return services;
    }

    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddSingleton<IScopeFactory, ScopeFactory>();

        services.AddValidation();
        services.AddDataServices();

        return services;
    }
}