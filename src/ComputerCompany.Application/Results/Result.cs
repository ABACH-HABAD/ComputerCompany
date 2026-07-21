using ComputerCompany.Application.Results.Json;
using System.Text;

namespace ComputerCompany.Application.Results;

public class Result
{
    public bool IsSuccess { get; protected init; }
    public string Message { get; protected init; } = string.Empty;

    protected Result()
    {

    }

    public static Result Success()
    {
        return new Result { IsSuccess = true };
    }

    public static Result Fail(string message)
    {
        return new Result { IsSuccess = false, Message = message };
    }

    public static Result Fail(params string[] messages)
    {
        StringBuilder stringBuilder = new();
        foreach (string message in messages)
        {
            stringBuilder.Append(message);
        }

        return new Result { IsSuccess = false, Message = stringBuilder.ToString() };
    }

    public static Result Fail(IEnumerable<string> messages)
    {
        StringBuilder stringBuilder = new();
        foreach (string message in messages)
        {
            stringBuilder.Append(message);
        }

        return new Result { IsSuccess = false, Message = stringBuilder.ToString() };
    }

    internal static Result FromJson(ResultJson resultJson)
    {
        Result result = new()
        {
            IsSuccess = resultJson.IsSuccess,
            Message = resultJson.Message,
        };

        return result;
    }
}