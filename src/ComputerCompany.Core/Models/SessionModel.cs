namespace ComputerCompany.Core.Models;

public record SessionModel(Guid Id, string Refresh, AccountModel Account, string Ip) : BaseModel(Id);