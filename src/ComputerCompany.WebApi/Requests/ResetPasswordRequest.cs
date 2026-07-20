namespace ComputerCompany.WebApi.Requests;

public record ResetPasswordRequest(Guid Id, string OldHashedPassword, string NewHashedPassword, bool ForceMode) : BaseRequest();