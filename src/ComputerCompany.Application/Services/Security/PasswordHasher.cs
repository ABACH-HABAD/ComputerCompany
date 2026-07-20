using System.Security.Cryptography;
using System.Text;
using ComputerCompany.Application.Abstractions.Services.Security;

namespace ComputerCompany.Application.Services.Security;

public class PasswordHasher : IPasswordHasherService
{
    public string HashPassword(string password)
    {
        byte[] source = Encoding.UTF8.GetBytes(password);
        byte[] bytes = SHA256.HashData(source);
        string hash = Convert.ToBase64String(bytes);
        return hash;
    }
}