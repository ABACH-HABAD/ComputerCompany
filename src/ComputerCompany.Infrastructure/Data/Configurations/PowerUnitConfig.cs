using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class PowerUnitConfig : IEntityTypeConfiguration<PowerUnitEntity>
{

    public void Configure(EntityTypeBuilder<PowerUnitEntity> builder)
    {
        builder.ToTable("PowerUnits").HasKey(powerunit => powerunit.Id);

        builder.HasIndex(powerunit => powerunit.Id).IsUnique();
        builder.HasIndex(powerunit => powerunit.Name).IsUnique();
        builder.HasIndex(powerunit => powerunit.Model).IsUnique();

        builder
        .Property(powerunit => powerunit.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(powerunit => powerunit.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(powerunit => powerunit.Description)
        .HasMaxLength(500);

        builder
        .Property(powerunit => powerunit.Count)
        .IsRequired();

        builder
        .Property(powerunit => powerunit.Price)
        .IsRequired();

        builder.Property(powerunit => powerunit.FormFactor).IsRequired().HasMaxLength(10);
        builder.Property(powerunit => powerunit.Certification).IsRequired().HasMaxLength(20);
        builder.Property(powerunit => powerunit.Power).IsRequired();
    }
}