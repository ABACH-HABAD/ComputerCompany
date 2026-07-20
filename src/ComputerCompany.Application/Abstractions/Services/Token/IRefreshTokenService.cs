using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Token;

public interface IRefreshTokenService
{
    public string CreateToken();
}