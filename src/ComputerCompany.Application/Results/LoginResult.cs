using ComputerCompany.Application.Results.Json;

namespace ComputerCompany.Application.Results;

public class LoginResult : Result
{
    public string Accept { get; protected init; } = string.Empty;
    public string Refresh { get; protected init; } = string.Empty;

    private LoginResult()
    {

    }

    public static LoginResult Success(string accept, string refresh)
    {
        return new LoginResult { IsSuccess = true, Accept = accept, Refresh = refresh };
    }

    public static new LoginResult Fail(string message)
    {
        return new LoginResult { IsSuccess = false, Message = message };
    }
    internal static LoginResult FromJson(LoginResultJson loginResultJson)
    {
        LoginResult loginResult = new()
        {
            IsSuccess = loginResultJson.IsSuccess,
            Message = loginResultJson.Message,
            Accept = loginResultJson.Accept,
            Refresh = loginResultJson.Refresh
        };

        return loginResult;
    }
}