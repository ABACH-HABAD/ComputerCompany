using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class RoleConfig : IEntityTypeConfiguration<RoleEntity>
{

    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles").HasKey(role => role.Id);

        builder.HasIndex(role => role.Id).IsUnique();
        builder.HasIndex(role => role.Name).IsUnique();

        builder.HasMany(role => role.Accounts).WithOne(account => account.Role).HasForeignKey(account => account.RoleId);
    }
}