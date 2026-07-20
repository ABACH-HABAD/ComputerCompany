using ComputerCompany.Application.Abstractions.Services.Security;
using ComputerCompany.Application.Client.Abstractions.Servies.Token;
using ComputerCompany.Application.Results;
using System.IO;
using System.Security.Cryptography;

namespace ComputerCompany.Presentation.Services.Token;

public class TokenStorageService : ITokenStorageService
{
    protected const string _foldername = "ComputerComapny";

    protected readonly IEncryption _encryption;
    protected readonly string _folderpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _foldername);

    protected string? _cachedAccessToken = null;
    protected string? _cachedRefreshToken = null;

    public event EventHandler<EventArgs>? TokenStorageChanged;

    public TokenStorageService(IEncryption encryption)
    {
        _encryption = encryption;
        if (!Directory.Exists(_folderpath)) Directory.CreateDirectory(_folderpath);
    }

    public async Task SaveTokensAsync(LoginResult login)
    {
        _cachedAccessToken = login.Accept;
        _cachedRefreshToken = login.Refresh;

        await SaveTokenAsync(Path.Combine(_folderpath, "access.dat"), _cachedAccessToken);
        await SaveTokenAsync(Path.Combine(_folderpath, "refresh.dat"), _cachedRefreshToken);

        TokenStorageChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        _cachedAccessToken ??= await LoadTokenAsync(Path.Combine(_folderpath, "access.dat"));
        return _cachedAccessToken;
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        _cachedRefreshToken ??= await LoadTokenAsync(Path.Combine(_folderpath, "refresh.dat"));
        return _cachedRefreshToken;
    }

    protected async Task SaveTokenAsync(string filepath, string token)
    {
        byte[] bytes = _encryption.Encrypt(token);
        await File.WriteAllBytesAsync(filepath, bytes);
    }

    protected async Task<string?> LoadTokenAsync(string filepath)
    {
        byte[] bytes = await File.ReadAllBytesAsync(filepath);
        try
        {
            return _encryption.Decrypt(bytes);
        }
        catch (CryptographicException)
        {
            return null;
        }
    }
}