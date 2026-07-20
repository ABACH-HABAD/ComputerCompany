using ComputerCompany.Infrastructure.Data;
using ComputerCompany.WebApi.Middleware;

namespace ComputerCompany.WebApi;

public class Program
{
    public static IServiceProvider ProgramServices { get; private set; } = null!;

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddHttpLogging();
        builder.Services.AddLogging();

        builder.WebHost.ConfigureKestrel(options =>
        {
            //options.ListenAnyIP(8080);
            //options.ListenLocalhost(5003);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        builder.Services.AddSecurity(builder);
        builder.Services.AddInfrastucture(builder);
        builder.Services.AddBusinessLogic();
        builder.Services.AddCustomMiddleware();

        WebApplication app = builder.Build();

        ProgramServices = app.Services;

        app.UseCustomMiddleware();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpLogging();
        

        app.MapControllers();

        using (IServiceScope scope = ProgramServices.CreateScope())
        {
            ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            ApplicationContext applicationContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            DataBaseConnectionString dataBaseConnectionString = scope.ServiceProvider.GetRequiredService<DataBaseConnectionString>();

            int maxTryesCount = 10;
            int tryesCount = 1;
            while (tryesCount < maxTryesCount)
            {
                try
                {
                    applicationContext.Database.EnsureCreated();
                    logger.LogInformation("Приложение успешно запущенно");
                    break;
                }
                catch
                {
                    logger.LogError("Не удалось подключится к базе даных, попытка {TryesCount} из {MaxTryesCount}", tryesCount, maxTryesCount);
                    Thread.Sleep(5000);
                    tryesCount++;
                }
            }
        }

        app.Run();
    }
}