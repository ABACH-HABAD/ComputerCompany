namespace ComputerCompany.Infrastructure.Data.Entities;

internal class SessionEntity : BaseEntity
{
    public string Refresh { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;

    public Guid AccountId { get; set; }

    public AccountEntity Account { get; set; } = null!;
}