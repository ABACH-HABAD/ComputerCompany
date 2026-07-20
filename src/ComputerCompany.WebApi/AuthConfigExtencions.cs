using ComputerCompany.Application.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ComputerCompany.WebApi;

public static class AuthConfigExtencions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        IConfigurationSection jwtConfig = config.GetSection("JwtSettings");

        JwtSettings jwtSettings = new(jwtConfig["SecretKey"] ?? throw new Exception("JWT ключ не настроен"),
                                       jwtConfig["Issuer"] ?? throw new Exception("Issuer не настроен"),
                                       jwtConfig["Audience"] ?? throw new Exception("Audience не настроен"),
                                       5);

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

        services.AddSingleton(jwtSettings);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    RequireSignedTokens = true,
                    ValidateSignatureLast = false,
                    ConfigurationManager = null
                };
            });

        services.AddAuthorization();

        return services;
    }
}
