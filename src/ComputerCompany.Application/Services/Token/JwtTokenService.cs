using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ComputerCompany.Application.Abstractions.Services.Token;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Token;

public class JwtTokenService(JwtSettings settings) : IJwtTokenService
{
    public string CreateToken(AccountModel account)
    {
        if (account.Login == null) throw new Exception("У аккаунта должен быть логин");
        if (account.Role == null) throw new Exception("У аккаунта должна быть роль");

        string NameId = account.Id.ToString();

        Claim[] claims =
        [
            new(ClaimTypes.NameIdentifier, NameId),
            new (ClaimTypes.Email, account.Login),
            new(ClaimTypes.Role, account.Role.Name)
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(settings.SecretKey));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new
        (
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            signingCredentials: creds,
            expires: DateTime.Now.AddMinutes(settings.ExpiryMinutes)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}