using System.Net.Http.Json;

namespace ComputerCompany.Application.Results.Json;

public static class ResultExtencions
{
    public const string JsonParseErrorMessage = "Данные с сервера прочитать не удалось";

    private async static Task<(bool result, T? output)> TryReadFromJsonAsync<T>(HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        try
        {
            T? outpoutValue = await httpContent.ReadFromJsonAsync<T>(cancellationToken);
            return (true, outpoutValue);
        }
        catch
        {
            return (false, default);
        }
    }

    public async static Task<Result> GetResultFromJsonAsync(this HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        (bool result, ResultJson? output) = await TryReadFromJsonAsync<ResultJson>(httpContent, cancellationToken);
        if (result == false || output == null) return Result.Fail(JsonParseErrorMessage);

        return Result.FromJson(output);
    }

    public async static Task<DataResult<T>> GetDataResultFromJsonAsync<T>(this HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        (bool result, DataResultJson<T>? output) = await TryReadFromJsonAsync<DataResultJson<T>>(httpContent, cancellationToken);
        if (result == false || output == null) return DataResult<T>.Fail(JsonParseErrorMessage);

        return DataResult<T>.FromJson(output);
    }

    public async static Task<LoginResult> GetLoginResultFromJsonAsync(this HttpContent httpContent, CancellationToken cancellationToken = default)
    {
        (bool result, LoginResultJson? output) = await TryReadFromJsonAsync<LoginResultJson>(httpContent, cancellationToken);
        if (result == false || output == null) return LoginResult.Fail(JsonParseErrorMessage);

        return LoginResult.FromJson(output);
    }
}