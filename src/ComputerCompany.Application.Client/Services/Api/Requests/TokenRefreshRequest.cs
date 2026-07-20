namespace ComputerCompany.Application.Client.Services.Api.Requests;

public record TokenRefreshRequest(string RefreshToken) : BaseRequest();