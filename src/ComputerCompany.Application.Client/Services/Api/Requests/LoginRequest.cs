namespace ComputerCompany.Application.Client.Services.Api.Requests;

public record LoginRequest(string Login, string HashedPassword) : BaseRequest();