using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class MotherboardConfig : IEntityTypeConfiguration<MotherboardEntity>
{

    public void Configure(EntityTypeBuilder<MotherboardEntity> builder)
    {
        builder.ToTable("Motherboards").HasKey(motherboard => motherboard.Id);

        builder.HasIndex(motherboard => motherboard.Id).IsUnique();
        builder.HasIndex(motherboard => motherboard.Name).IsUnique();
        builder.HasIndex(motherboard => motherboard.Model).IsUnique();

        builder
        .Property(motherboard => motherboard.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(motherboard => motherboard.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(motherboard => motherboard.Description)
        .HasMaxLength(500);

        builder
        .Property(motherboard => motherboard.Count)
        .IsRequired();

        builder
        .Property(motherboard => motherboard.Price)
        .IsRequired();

        builder.Property(motherboard => motherboard.Socket).IsRequired().HasMaxLength(10);
        builder.Property(motherboard => motherboard.Chipset).IsRequired().HasMaxLength(10);
    }
}