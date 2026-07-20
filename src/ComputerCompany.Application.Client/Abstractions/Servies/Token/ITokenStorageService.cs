using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Token;

public interface ITokenStorageService
{
    public event EventHandler<EventArgs>? TokenStorageChanged;

    public Task<string?> GetAccessTokenAsync();
    public Task<string?> GetRefreshTokenAsync();

    public Task SaveTokensAsync(LoginResult login);
}