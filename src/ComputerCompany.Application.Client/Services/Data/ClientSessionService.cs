using System.Net.Http.Json;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Services.Api.Requests;
using ComputerCompany.Application.Results.Json;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientSessionService
(
    ITokenStorageService tokenStorageService,
    IValidator<SessionModel> validator,
    IApiClientService apiClient
) :
BaseClientService<SessionModel>("session", validator, apiClient),
ISessionService
{
    protected readonly ITokenStorageService _tokenStorageService = tokenStorageService;

    public async Task<DataResult<SessionModel>> GetByAccountAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult apiResult = await _apiClient.GetAsync(_address + $"search?accountId={accountId}", cancellationToken: cancellationToken);

            if (!apiResult.IsSuccess) return DataResult<SessionModel>.Fail(apiResult.Message);
            else
            {
                DataResult<SessionModel>? dataResult = await apiResult.Data.ReadFromJsonAsync<DataResult<SessionModel>>(cancellationToken);
                if (dataResult == null) return DataResult<SessionModel>.Fail("Данные с сервера прочитать не удалось");

                return dataResult;
            }
        }
        catch (Exception ex)
        {
            return DataResult<SessionModel>.Fail(ex.Message);
        }
    }

    public async Task<LoginResult> RefreshTokensAsync(string? refreshToken, string? ip = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (refreshToken == null || refreshToken == string.Empty) refreshToken = await _tokenStorageService.GetRefreshTokenAsync();
            if (refreshToken == null) return LoginResult.Fail("Токен отсутствует");

            TokenRefreshRequest tokenRefreshRequest = new(refreshToken);
            ApiResult apiResult = await _apiClient.PutAsync(_address + "/refresh", tokenRefreshRequest, retryIfUnauthorized: false, cancellationToken: cancellationToken);

            if (!apiResult.IsSuccess) return LoginResult.Fail(apiResult.Message);
            else
            {
                LoginResult loginResult = await apiResult.Data.GetLoginResultFromJsonAsync(cancellationToken);

                await _tokenStorageService.SaveTokensAsync(loginResult);

                return loginResult;
            }
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }

    public async Task<Result> LogoutAsync(Guid accountId, string? ip = null, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult apiResult = await _apiClient.DeleteAsync(_address + "/logout/" + accountId, cancellationToken: cancellationToken);

            return apiResult;
        }
        catch (Exception ex)
        {
            return LoginResult.Fail(ex.Message);
        }
    }
}