using ComputerCompany.Application.Abstractions.Services.Token;
using System.Security.Cryptography;

namespace ComputerCompany.Application.Services.Token;

public class RefreshTokenService : IRefreshTokenService
{
    public string CreateToken()
    {
        byte[] randomBytes = new byte[64];
        using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}