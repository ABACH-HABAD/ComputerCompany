namespace ComputerCompany.WebApi.Requests;

public record TokenRefreshRequest(string RefreshToken) : BaseRequest();