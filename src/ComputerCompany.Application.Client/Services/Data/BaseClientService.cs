using System.Net.Http.Json;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Services.Api.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Services.Api.Requests;
using ComputerCompany.Application.Results.Json;
using System.Net;

namespace ComputerCompany.Application.Client.Services.Data;

public abstract class BaseClientService<T>(string address, IValidator<T> validator, IApiClientService apiClient) : IDataService<T> where T : BaseModel
{
    protected readonly string _address = address;
    protected readonly IApiClientService _apiClient = apiClient;
    protected readonly IValidator<T> _validator = validator;

    private static Result ResultByStatusCode(ApiResult httpResult)
    {
        return httpResult.StatusCode switch
        {
            HttpStatusCode.NotFound => Result.Fail("Ресурс по данному запросу не найден"),
            HttpStatusCode.Forbidden => Result.Fail("Отказано в доступе"),
            HttpStatusCode.Unauthorized => Result.Fail("Время сессии исекло, пожалуйста перезайдите"),
            HttpStatusCode.MethodNotAllowed => Result.Fail("Метод не доступен"),
            _ => httpResult
        };
    }

    private static DataResult<TDataType> ToDataResult<TDataType>(Result result) => DataResult<TDataType>.Fail(result.Message);

    public virtual async Task<DataResult<T>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult httpResult = await _apiClient.GetAsync(_address + '/' + id, cancellationToken: cancellationToken);
            if (httpResult.IsSuccess)
            {
                HttpContent httpContent = httpResult.Data;
                DataResult<T> dataResult = await httpContent.GetDataResultFromJsonAsync<T>(cancellationToken);

                if (!dataResult.IsSuccess && dataResult.Message == ResultExtencions.JsonParseErrorMessage)
                {
                    return ToDataResult<T>(ResultByStatusCode(httpResult));
                }

                return dataResult;
            }
            else
            {
                return DataResult<T>.Fail(httpResult.Message);
            }
        }
        catch (Exception ex)
        {
            return DataResult<T>.Fail(ex.Message);
        }
    }

    public virtual async Task<DataResult<List<T>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult httpResult = await _apiClient.GetAsync(_address + "/all", cancellationToken: cancellationToken);
            if (httpResult.IsSuccess)
            {
                HttpContent httpContent = httpResult.Data;
                DataResult<List<T>> dataResult = await httpContent.GetDataResultFromJsonAsync<List<T>>(cancellationToken);

                if (!dataResult.IsSuccess && dataResult.Message == ResultExtencions.JsonParseErrorMessage)
                {
                    return ToDataResult<List<T>>(ResultByStatusCode(httpResult));
                }
                
                return dataResult;
            }
            else
            {
                return DataResult<List<T>>.Fail(httpResult.Message);
            }
        }
        catch (Exception ex)
        {
            return DataResult<List<T>>.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> AddAsync(T data, CancellationToken cancellationToken = default)
    {
        try
        {
            Result validationResult = _validator.Validate(data);
            if (!validationResult.IsSuccess) return validationResult;

            DataRequest<T> request = new(data);

            ApiResult httpResult = await _apiClient.PostAsync(_address, request, cancellationToken: cancellationToken);

            if (httpResult.IsSuccess)
            {
                HttpContent httpContent = httpResult.Data;
                Result result = await httpContent.GetResultFromJsonAsync(cancellationToken);

                if (!result.IsSuccess && result.Message == ResultExtencions.JsonParseErrorMessage)
                {
                    return ResultByStatusCode(httpResult);
                }

                return result;
            }
            else
            {
                return Result.Fail(httpResult.Message);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> UpdateAsync(T data, CancellationToken cancellationToken = default)
    {
        try
        {
            Result validationResult = _validator.Validate(data);
            if (!validationResult.IsSuccess) return validationResult;

            DataRequest<T> request = new(data);

            ApiResult httpResult = await _apiClient.PutAsync(_address, request, cancellationToken: cancellationToken);

            if (httpResult.IsSuccess)
            {
                HttpContent httpContent = httpResult.Data;
                Result result = await httpContent.GetResultFromJsonAsync(cancellationToken);

                if (!result.IsSuccess && result.Message == ResultExtencions.JsonParseErrorMessage)
                {
                    return ResultByStatusCode(httpResult);
                }

                return result;
            }
            else
            {
                return Result.Fail(httpResult.Message);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }

    public virtual async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            ApiResult httpResult = await _apiClient.DeleteAsync(_address + '/' + id, cancellationToken: cancellationToken);

            if (httpResult.IsSuccess)
            {
                HttpContent httpContent = httpResult.Data;
                Result result = await httpContent.GetResultFromJsonAsync(cancellationToken);

                if (!result.IsSuccess && result.Message == ResultExtencions.JsonParseErrorMessage)
                {
                    return ResultByStatusCode(httpResult);
                }

                return result;
            }
            else
            {
                return Result.Fail(httpResult.Message);
            }
        }
        catch (Exception ex)
        {
            return Result.Fail(ex.Message);
        }
    }
}