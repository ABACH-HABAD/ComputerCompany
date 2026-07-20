using System.Net;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.Services.Api.Results;

public class ApiResult : DataResult<HttpContent>
{
    public HttpStatusCode StatusCode { get; init; }

    private ApiResult()
    {

    }

    public static ApiResult FormResult(HttpResponseMessage httpResponseMessage)
    {
        return new ApiResult
        {
            IsSuccess = true,
            StatusCode = httpResponseMessage.StatusCode,
            Message = httpResponseMessage.ToString(),
            Data = httpResponseMessage.Content
        };
    }

    public static new ApiResult Fail(string message)
    {
        return new ApiResult
        {
            IsSuccess = false,
            Message = message
        };
    }
}