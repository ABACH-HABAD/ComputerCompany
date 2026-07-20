using System.Net;
using System.Net.Http.Json;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Results.Json;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Client.Services.Api.Requests;
using ComputerCompany.Application.Client.Services.Api.Results;

namespace ComputerCompany.Application.Client.Services.Api;

public class ApiService : IApiClientService
{
    protected const string Api = "/api/";
    protected const string RefreshPath = "session/refresh";

    private bool _isTokenLoaded;

    protected readonly ApiSettings _httpSettings;
    protected readonly ITokenStorageService _tokenStorageService;
    protected readonly HttpClient _client;

    public ApiService(ApiSettings apiSettings, ITokenStorageService tokenStorageService)
    {
        _httpSettings = apiSettings;
        _tokenStorageService = tokenStorageService;
        _client = new()
        {
            Timeout = TimeSpan.FromSeconds(apiSettings.Timeout),
            BaseAddress = new Uri(apiSettings.Address)
        };
    }

    public async Task LoadTokenAsync()
    {
        string? accessToken = await _tokenStorageService.GetAccessTokenAsync();
        if (accessToken != null)
        {
            _client.DefaultRequestHeaders.Authorization = new("Bearer", accessToken);
            _isTokenLoaded = true;
        }
        else
        {
            _client.DefaultRequestHeaders.Authorization = null;
            _isTokenLoaded = false;
        }
    }

    public async void OnTokenChanged(object sender, EventArgs eventArgs)
    {
        await LoadTokenAsync();
    }

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
    public async Task<ApiResult> SendRequestAsync(Func<string, CancellationToken, Task<HttpResponseMessage>> sendRequestAsyncFunc, string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        //Если jwt токен авторизации не загружен, то загружаем его
        if (!_isTokenLoaded) await LoadTokenAsync();

        //Формируем полный адрес 
        string fulladdress = _httpSettings.Address + Api + address;

        try
        {
            //Отправляем запрос
            HttpResponseMessage responce = await sendRequestAsyncFunc(fulladdress, cancellationToken);

            //Повтор если не авторизирован нужен в тех случаях, когда сессия истекла, и нам необходимо автоматически её обновить
            //В зпросах на авторизацию и регистрацию это не требуется, ибо изначально мы ещё не авторизированны
            if (retryIfUnauthorized && responce.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Если наша сессия истекла

                //Получаем токен из локального хранилища
                string? refreshToken = await _tokenStorageService.GetRefreshTokenAsync();

                //Если токена в хранилище не оказалось, то авторизоваться мы не сможем
                //Возвращаем исходную ошибку
                if (refreshToken == null) return ApiResult.FormResult(responce);

                //Формируем запрос
                TokenRefreshRequest tokenRefreshRequest = new(refreshToken);

                //Отправляем его
                ApiResult tokenRefreshResponce = await PutAsync(RefreshPath, tokenRefreshRequest, retryIfUnauthorized: false, cancellationToken);

                //Если авторизировать удалось
                if (tokenRefreshResponce.StatusCode != HttpStatusCode.Unauthorized)
                {
                    //Формируем результат
                    LoginResult loginResult = await tokenRefreshResponce.Data.GetLoginResultFromJsonAsync(cancellationToken);

                    //Сохраняем токены в памяти
                    await _tokenStorageService.SaveTokensAsync(loginResult);

                    //Повторяем отправку исходного запроса
                    HttpResponseMessage retryResponce = await sendRequestAsyncFunc(fulladdress, cancellationToken);

                    //Отдаём результат
                    return ApiResult.FormResult(retryResponce);
                }

                //Если авторизоватсья всё равно не получилось
                //Истекло время сессии или типо того
                //Вернём исходную ошибку
                else return ApiResult.FormResult(responce);
            }
            //Если сессия ещё действует
            //Или у нас запрос на авторизацию
            //То формируем результат запроса
            else return ApiResult.FormResult(responce);
        }
        catch (Exception ex)
        {
            return ApiResult.Fail(ex.Message);
        }
    }

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
    public async Task<ApiResult> SendRequestAsync(Func<string, HttpContent, CancellationToken, Task<HttpResponseMessage>> sendRequestAsyncFunc, string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        //Если jwt токен авторизации не загружен, то загружаем его
        if (!_isTokenLoaded) await LoadTokenAsync();

        //Формируем полный адрес 
        string fulladdress = _httpSettings.Address + Api + address;

        try
        {
            //Отправляем запрос
            HttpResponseMessage responce = await sendRequestAsyncFunc(fulladdress, JsonContent.Create(data), cancellationToken);

            //Повтор если не авторизирован нужен в тех случаях, когда сессия истекла, и нам необходимо автоматически её обновить
            //В зпросах на авторизацию и регистрацию это не требуется, ибо изначально мы ещё не авторизированны
            if (retryIfUnauthorized && responce.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Если наша сессия истекла

                //Получаем токен из локального хранилища
                string? refreshToken = await _tokenStorageService.GetRefreshTokenAsync();

                //Если токена в хранилище не оказалось, то авторизоваться мы не сможем
                //Возвращаем исходную ошибку
                if (refreshToken == null) return ApiResult.FormResult(responce);

                //Формируем запрос
                TokenRefreshRequest tokenRefreshRequest = new(refreshToken);

                //Отправляем его
                ApiResult tokenRefreshResponce = await PutAsync(RefreshPath, tokenRefreshRequest, retryIfUnauthorized: false, cancellationToken);

                //Если авторизировать удалось
                if (tokenRefreshResponce.StatusCode != HttpStatusCode.Unauthorized)
                {
                    //Формируем результат
                    LoginResult loginResult = await tokenRefreshResponce.Data.GetLoginResultFromJsonAsync(cancellationToken);

                    //Сохраняем токены в памяти
                    await _tokenStorageService.SaveTokensAsync(loginResult);

                    //Повторяем отправку исходного запроса
                    HttpResponseMessage retryResponce = await sendRequestAsyncFunc(fulladdress, JsonContent.Create(data), cancellationToken);

                    //Отдаём результат
                    return ApiResult.FormResult(retryResponce);
                }

                //Если авторизоватсья всё равно не получилось
                //Истекло время сессии или типо того
                //Вернём исходную ошибку
                else return ApiResult.FormResult(responce);
            }
            //Если сессия ещё действует
            //Или у нас запрос на авторизацию
            //То формируем результат запроса
            else return ApiResult.FormResult(responce);
        }
        catch (Exception ex)
        {
            return ApiResult.Fail(ex.Message);
        }
    }

    public async Task<ApiResult> GetAsync(string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync(_client.GetAsync, address, retryIfUnauthorized, cancellationToken);
    }

    public async Task<ApiResult> PostAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync(_client.PostAsync, address, data, retryIfUnauthorized, cancellationToken);
    }

    public async Task<ApiResult> PutAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync(_client.PutAsync, address, data, retryIfUnauthorized, cancellationToken);
    }

    public async Task<ApiResult> PatchAsync(string address, object data, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync(_client.PatchAsync, address, data, retryIfUnauthorized, cancellationToken);
    }

    public async Task<ApiResult> DeleteAsync(string address, bool retryIfUnauthorized = true, CancellationToken cancellationToken = default)
    {
        return await SendRequestAsync(_client.DeleteAsync, address, retryIfUnauthorized, cancellationToken);
    }
}