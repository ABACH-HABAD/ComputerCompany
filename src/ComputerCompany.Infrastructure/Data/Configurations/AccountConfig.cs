using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class AccountConfig : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("Accounts").HasKey(account => account.Id);

        builder.HasIndex(account => account.Id).IsUnique();
        builder.HasIndex(account => account.Login).IsUnique();

        builder
        .Property(account => account.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(account => account.Login)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(account => account.Password)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .HasOne(account => account.Role)
        .WithMany(role => role.Accounts)
        .HasForeignKey(account => account.RoleId);

        builder
        .HasMany(account => account.Assemblies)
        .WithOne(assembly => assembly.Account)
        .HasForeignKey(assembly => assembly.AccountId);
    }
}