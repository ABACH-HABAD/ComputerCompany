namespace ComputerCompany.Infrastructure.Data.Entities;

internal class ReviewEntity : BaseEntity
{
    public  string Message { get; set; } = string.Empty;
    public  int Stars { get; set; }

    public  Guid SenderId { get; set; }

    public AccountEntity Sender { get; set; } = null!;
}