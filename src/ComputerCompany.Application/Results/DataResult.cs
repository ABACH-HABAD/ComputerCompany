using ComputerCompany.Application.Results.Json;

namespace ComputerCompany.Application.Results;

public class DataResult<T> : Result
{
    public T Data { get; protected init; } = default!;

    protected DataResult()
    {

    }

    public static DataResult<T> Success(T data)
    {
        return new DataResult<T> { IsSuccess = true, Data = data };
    }

    public static DataResult<T> Fail(string message, T data = default!)
    {
        return new DataResult<T> { IsSuccess = false, Message = message, Data = data };
    }
    internal static DataResult<T> FromJson(DataResultJson<T> dataResultJson)
    {
        DataResult<T> dataResult = new()
        {
            IsSuccess = dataResultJson.IsSuccess,
            Message = dataResultJson.Message,
            Data = dataResultJson.Data
        };

        return dataResult;
    }
}