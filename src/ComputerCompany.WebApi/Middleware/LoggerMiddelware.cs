namespace ComputerCompany.WebApi.Middleware;

public class LoggerMiddelware(ILogger<LoggerMiddelware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string method = context.Request.Method;
        string requestUri = context.Request.Path;
        string ip = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;


        logger.LogInformation("Получен {Method} запрос {RequestUri} с IP: {Ip}", method, requestUri, ip);

        await next.Invoke(context);

        string code = context.Response.StatusCode.ToString();
        string hasError = "успешно";
        LogLevel level = LogLevel.Information;
        if (context.Response.StatusCode >= 500)
        {
            hasError = "с ошибкой";
            level = LogLevel.Error;
        }

        logger.Log(level, "Запрос {Method} {RequestUri} с IP: {Ip} обработан {HasError}. Код: {Code}", method, requestUri, ip, hasError, code);
    }
}