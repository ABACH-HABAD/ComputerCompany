using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Token;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class AccountService
(
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService,
    IValidator<AccountModel> accountValidator,
    IEmailValidator emailValidator,
    IAccountRepository repository,
    ISessionService sessionService,
    IRoleCache roleCache
) :
BaseDataService<AccountModel, IAccountRepository>(accountValidator, repository, "аккаунт"),
IAccountService
{
    public async Task<LoginResult> LoginAsync(string login, string password, string? ip = null, CancellationToken cancellationToken = default)
    {
        //Валидация
        if (ip == null) return LoginResult.Fail("Недействительный IP адрес");

        Result loginValidationResult = emailValidator.Validate(login);
        if (!loginValidationResult.IsSuccess) return LoginResult.Fail(loginValidationResult.Message);

        //Валидировать пароль НЕНАДО!
        //Он приходит на сервер в виде хэш-кода

        try
        {
            //Получаем аккаунт из базы
            AccountModel? account = await _repository.GetByLoginAndPasswordAsync(login, password, cancellationToken);
            if (account == null) return LoginResult.Fail("Аккаунта с таким логином и паролем не существует");

            //Завершаем старую сессию, если она есть
            await sessionService.LogoutAsync(account.Id, ip, cancellationToken);

            //Генерируем токены авторизации и обновления
            string refresh = refreshTokenService.CreateToken();
            string access = jwtTokenService.CreateToken(account);

            //Начинаем новую сессию
            Result sessionResult = await sessionService.AddAsync(new SessionModel(Guid.NewGuid(), refresh, account, ip), cancellationToken);
            if (!sessionResult.IsSuccess) return LoginResult.Fail(sessionResult.Message);

            return LoginResult.Success(access, refresh);
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<LoginResult> RegistrateAsync(string login, string password, string repeatPassword, string? ip = null, bool saveLoginData = true, CancellationToken cancellationToken = default)
    {
        //Валидация
        if (ip == null) return LoginResult.Fail("Недействительный IP адрес");

        if (password != repeatPassword) return LoginResult.Fail("Пароли не совпадают");

        Result loginValidationResult = emailValidator.Validate(login);
        if (!loginValidationResult.IsSuccess) return LoginResult.Fail(loginValidationResult.Message);

        //Валидировать пароль НЕНАДО!
        //Он приходит на сервер в виде хэш-кода

        try
        {
            //Создаём новый аккаунт
            await _repository.AddAsync
            (
                new AccountModel
                (
                    default,
                    login,
                    "Покупатель компьютеров",
                    roleCache.UserRole
                ),
                password,
                cancellationToken
            );

            //Находим его в базе
            AccountModel? accountModel = await _repository.GetByLoginAndPasswordAsync(login, password, cancellationToken) ?? throw new Exception("Ошибка при регистрации");

            //Генерируем токены авторизации и обновления
            string access = jwtTokenService.CreateToken(accountModel);
            string refresh = refreshTokenService.CreateToken();

            //Начинаем новую сессию
            Result sessionResult = await sessionService.AddAsync(new SessionModel(Guid.NewGuid(), refresh, accountModel, ip), cancellationToken);
            if (!sessionResult.IsSuccess) return LoginResult.Fail(sessionResult.Message);

            //Не сохраняем данные, если надо (по умолчанию true)
            if (saveLoginData == false) return LoginResult.Success(string.Empty, string.Empty);

            return LoginResult.Success(access, refresh);
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<Result> ResetPasswordAsync(Guid id, string oldPassword, string newPassword, bool forceChange = false, CancellationToken cancellationToken = default)
    {
        try
        {
            //Валидировать пароль НЕНАДО!
            //Он приходит на сервер в виде хэш-кода

            //Принудительное изменение пароля (forceChange) доступно только администратору. При нём проверять подлинность старого пароля не требуется
            //Это необходимо если аккаунт был украден и других способов восстановления нету
            //Права администратора проверяются в контроллере
            if (!forceChange)
            {
                bool isPasswordCorrect = await _repository.CheckPasswordAsync(id, oldPassword, cancellationToken);
                if (!isPasswordCorrect) return Result.Fail("Пароли не совпадают");
            }

            await _repository.ChangePasswordAsync(id, newPassword, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}