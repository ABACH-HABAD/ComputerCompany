namespace ComputerCompany.Application.Client.Services.Api.Requests;

public record ResetPasswordRequest(Guid Id, string OldHashedPassword, string NewHashedPassword, bool ForceMode = false) : BaseRequest();