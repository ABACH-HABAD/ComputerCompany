namespace ComputerCompany.Application.Abstractions.Services.Security;

public interface IEncryption
{
    public byte[] Encrypt(string data);
    public string Decrypt(byte[] bytes);
}