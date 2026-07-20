using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Token;

public interface IJwtTokenService
{
    public string CreateToken(AccountModel account);
}