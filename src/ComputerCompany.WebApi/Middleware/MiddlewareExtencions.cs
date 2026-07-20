namespace ComputerCompany.WebApi.Middleware;

public static class MiddlewareExtencions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<LoggerMiddelware>();

        return app;
    }

    /// <summary>
    /// Используй при регистрации в DI
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomMiddleware(this IServiceCollection services)
    {
        services.AddScoped<LoggerMiddelware>();

        return services;
    }
}