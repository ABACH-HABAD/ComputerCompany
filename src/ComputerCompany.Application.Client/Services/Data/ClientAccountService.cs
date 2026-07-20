using System.Net.Http.Json;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Abstractions.Services.Security;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Services.Api.Requests;
using System.Net;
using ComputerCompany.Application.Results.Json;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientAccountService
(
    ITokenStorageService tokenStorageService,
    IEmailValidator emailValidator,
    IPasswordValidator passwordValidator,
    IPasswordHasherService passwordHasherService,
    IValidator<AccountModel> validator,
    IApiClientService apiClient
) :
BaseClientService<AccountModel>("account", validator, apiClient),
IAccountService
{
    protected readonly ITokenStorageService _tokenStorageService = tokenStorageService;
    protected readonly IEmailValidator _emailValidator = emailValidator;
    protected readonly IPasswordValidator _passwordValidator = passwordValidator;
    protected readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

    public async Task<LoginResult> LoginAsync(string login, string password, string? ip = null, CancellationToken cancellationToken = default)
    {
        Result result;

        result = _emailValidator.Validate(login);
        if (!result.IsSuccess) return LoginResult.Fail(result.Message);

        result = _passwordValidator.Validate(password);
        if (!result.IsSuccess) return LoginResult.Fail(result.Message);

        try
        {
            string hashedPassword = _passwordHasherService.HashPassword(password);
            LoginRequest loginRequest = new(login, hashedPassword);

            ApiResult apiResult = await _apiClient.PostAsync(_address + "/login", loginRequest, retryIfUnauthorized: false, cancellationToken: cancellationToken);
            if (!apiResult.IsSuccess) return LoginResult.Fail(apiResult.Message);

            string msg = await apiResult.Data.ReadAsStringAsync(cancellationToken);
            LoginResult loginResult = await apiResult.Data.GetLoginResultFromJsonAsync(cancellationToken);

            await _tokenStorageService.SaveTokensAsync(loginResult);

            return loginResult;
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<LoginResult> RegistrateAsync(string login, string password, string repeatPassword, string? ip = null, bool saveLoginData = true, CancellationToken cancellationToken = default)
    {
        Result result;

        if (password != repeatPassword) return LoginResult.Fail("Пароли не совпадают");

        result = _emailValidator.Validate(login);
        if (!result.IsSuccess) return LoginResult.Fail(result.Message);

        result = _passwordValidator.Validate(password);
        if (!result.IsSuccess) return LoginResult.Fail(result.Message);

        try
        {
            string hashedPassword = _passwordHasherService.HashPassword(password);
            string repeatHashedPassword = _passwordHasherService.HashPassword(repeatPassword);
            RegistrationRequest registrationRequest = new(login, hashedPassword, repeatHashedPassword, saveLoginData);

            ApiResult apiResult = await _apiClient.PostAsync(_address + "/registrate", registrationRequest, retryIfUnauthorized: false, cancellationToken: cancellationToken);
            if (!apiResult.IsSuccess) return LoginResult.Fail(apiResult.Message);

            LoginResult loginResult = await apiResult.Data.GetLoginResultFromJsonAsync(cancellationToken);

            if (saveLoginData) await _tokenStorageService.SaveTokensAsync(loginResult);

            return loginResult;
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<Result> ResetPasswordAsync(Guid id, string oldPassword, string newPassword, bool forceChange = false, CancellationToken cancellationToken = default)
    {
        Result result;

        if (oldPassword == newPassword) return LoginResult.Fail("Пароли не должны совпадать");

        result = _passwordValidator.Validate(newPassword);
        if (!result.IsSuccess) return LoginResult.Fail(result.Message);

        try
        {
            string oldHashedPassword = _passwordHasherService.HashPassword(oldPassword);
            string newHashedPassword = _passwordHasherService.HashPassword(newPassword);
            ResetPasswordRequest request = new(id, oldHashedPassword, newHashedPassword, forceChange);

            ApiResult apiResult = await _apiClient.PatchAsync(_address + "/resetPassword", request, cancellationToken: cancellationToken);
            if (!apiResult.IsSuccess) return LoginResult.Fail(apiResult.Message);

            Result? passwordResetResult = await apiResult.Data.GetResultFromJsonAsync(cancellationToken);
            if (passwordResetResult == null) return LoginResult.Fail("Данные с сервера прочитать не удалось");

            return passwordResetResult;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}