namespace ComputerCompany.WebApi.Requests;

public record LoginRequest(string Login, string HashedPassword) : BaseRequest();