namespace ComputerCompany.WebApi.Requests;

public record RegistrationRequest(string Login, string HashedPassword, string RepeatHasedPassword, bool SaveLoginData = true) : LoginRequest(Login, HashedPassword);