using ComputerCompany.Application.Abstractions.Services.Security;
using System.Security.Cryptography;
using System.Text;

namespace ComputerCompany.Application.Client.Services.Security;

public class EncryptionService : IEncryption
{
    public byte[] Encrypt(string data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        byte[] encryptedBytes = ProtectedData.Protect(bytes, optionalEntropy: null, scope: DataProtectionScope.CurrentUser);

        return encryptedBytes;
    }

    public string Decrypt(byte[] bytes)
    {
        byte[] decryptedBytes = ProtectedData.Unprotect(bytes, optionalEntropy: null, scope: DataProtectionScope.CurrentUser);
        string data = Encoding.UTF8.GetString(decryptedBytes);

        return data;
    }
}