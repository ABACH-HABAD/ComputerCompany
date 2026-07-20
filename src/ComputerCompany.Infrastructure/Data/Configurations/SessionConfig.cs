using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class SessionConfig : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.ToTable("Sessions").HasKey(session => session.Id);

        builder.HasIndex(session => session.Id).IsUnique();
        builder.HasIndex(session => session.AccountId);
        builder.HasIndex(session => session.Refresh).IsUnique();

        builder.Property(session => session.Ip).IsRequired();

        builder.HasOne(session => session.Account).WithMany(account => account.Sessions).HasForeignKey(session => session.AccountId);
    }
}