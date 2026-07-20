using ComputerCompany.Application.Client.Services.Api.Results;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Api;

public interface IApiClientService
{
    /// <summary>
    /// Общий метод для отправки HTTP запросов  
    /// Подгружает токены авторизации  
    /// Формарует полный адрес  
    /// В случае если сессия завершена, обновляет токены авторизации, затем повторяет запрос  
    /// (Перегрузка для методов: GET, DELETE)
    /// </summary>
    /// <param name="sendRequestAsyncFunc">Делегат</param>
    /// <param name="address">Адрес</param>
    /// <param name="retryIfUnauthorized">Нужно ли обновлять сессию в случает неавторизации</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public Task<ApiResult> SendRequestAsync(Func<string, CancellationToken, Task<HttpResponseMessage>> sendRequestAsyncFunc, string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Общий метод для отправки HTTP запросов  
    /// Подгружает токены авторизации  
    /// Формарует полный адрес  
    /// В случае если сессия завершена, обновляет токены авторизации, затем повторяет запрос  
    /// (Перегрузка для методов: PUT, POST, PATCH)
    /// </summary>
    /// <param name="sendRequestAsyncFunc">Делегат</param>
    /// <param name="address">Адрес</param>
    /// <param name="data">Объект, который будет телом запроса</param>
    /// <param name="retryIfUnauthorized">Нужно ли обновлять сессию в случает неавторизации</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="sendRequestAsyncFunc"></param>
    /// <returns></returns>
    public Task<ApiResult> SendRequestAsync(Func<string, HttpContent, CancellationToken, Task<HttpResponseMessage>> sendRequestAsyncFunc, string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);

    public Task<ApiResult> GetAsync(string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);
    public Task<ApiResult> PostAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);
    public Task<ApiResult> PutAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);
    public Task<ApiResult> PatchAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);
    public Task<ApiResult> DeleteAsync(string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default);
}