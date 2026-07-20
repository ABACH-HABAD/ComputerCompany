namespace ComputerCompany.Application.Services.Token;

public record JwtSettings(string SecretKey, string Issuer, string Audience, double ExpiryMinutes);