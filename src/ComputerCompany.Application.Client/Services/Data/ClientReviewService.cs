using System.Net.Http.Json;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Security;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Client.Services.Api.Requests;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Results.Json;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientReviewService
(
    ITokenStorageService tokenStorageService,
    IEmailValidator emailValidator,
    IPasswordValidator passwordValidator,
    IPasswordHasherService passwordHasherService,
    IValidator<ReviewModel> validator,
    IApiClientService apiClient
) :
BaseClientService<ReviewModel>("review", validator, apiClient),
IReviewService
{
    protected readonly ITokenStorageService _tokenStorageService = tokenStorageService;
    protected readonly IEmailValidator _emailValidator = emailValidator;
    protected readonly IPasswordValidator _passwordValidator = passwordValidator;
    protected readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

    public async Task<DataResult<ReviewModel>> GetByAccountAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult apiResult = await _apiClient.GetAsync(_address + $"/search?accountId={accountId}", cancellationToken: cancellationToken);

            if (!apiResult.IsSuccess) return DataResult<ReviewModel>.Fail(apiResult.Message);
            else
            {
                DataResult<ReviewModel> dataResult = await apiResult.Data.GetDataResultFromJsonAsync<ReviewModel>(cancellationToken);

                return dataResult;
            }
        }
        catch (Exception ex)
        {
            return DataResult<ReviewModel>.Fail(ex.Message);
        }
    }

    public async Task<Result> SendReviewAsync(ReviewModel reviewModel, CancellationToken cancellationToken = default)
    {
        try
        {
            Result validationResult = _validator.Validate(reviewModel);
            if (!validationResult.IsSuccess) return validationResult;

            DataRequest<ReviewModel> request = new(reviewModel);

            ApiResult httpResult = await _apiClient.PutAsync(_address + "/send", request, cancellationToken: cancellationToken);

            return httpResult;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public async Task<DataResult<ReviewModel>> GetRandomAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult apiResult = await _apiClient.GetAsync(_address + $"/random", cancellationToken: cancellationToken);

            if (!apiResult.IsSuccess) return DataResult<ReviewModel>.Fail(apiResult.Message);
            else
            {
                DataResult<ReviewModel>? dataResult = await apiResult.Data.GetDataResultFromJsonAsync<ReviewModel>(cancellationToken);
                if (dataResult == null) return DataResult<ReviewModel>.Fail("Данные с сервера прочитать не удалось");

                return dataResult;
            }
        }
        catch (Exception ex)
        {
            return DataResult<ReviewModel>.Fail(ex.Message);
        }
    }

    public async Task<DataResult<List<ReviewModel>>> GetRandomRangeAsync(int range, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult apiResult = await _apiClient.GetAsync(_address + $"/random?range={range}", cancellationToken: cancellationToken);

            if (!apiResult.IsSuccess) return DataResult<List<ReviewModel>>.Fail(apiResult.Message);
            else
            {
                DataResult<List<ReviewModel>>? dataResult = await apiResult.Data.GetDataResultFromJsonAsync<List<ReviewModel>>(cancellationToken);
                if (dataResult == null) return DataResult<List<ReviewModel>>.Fail("Данные с сервера прочитать не удалось");

                return dataResult;
            }
        }
        catch (Exception ex)
        {
            return DataResult<List<ReviewModel>>.Fail(ex.Message);
        }
    }
}