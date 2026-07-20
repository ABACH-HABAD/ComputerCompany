namespace ComputerCompany.Application.Abstractions.Services.Security;

public interface IPasswordHasherService
{
    public string HashPassword(string password);
}