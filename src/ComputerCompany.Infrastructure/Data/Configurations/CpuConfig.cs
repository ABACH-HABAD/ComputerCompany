using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class CpuConfig : IEntityTypeConfiguration<CpuEntity>
{
    public void Configure(EntityTypeBuilder<CpuEntity> builder)
    {
        builder.ToTable("Cpus").HasKey(cpu => cpu.Id);

        builder.HasIndex(cpu => cpu.Id).IsUnique();
        builder.HasIndex(cpu => cpu.Name).IsUnique();
        builder.HasIndex(cpu => cpu.Model).IsUnique();

        builder
        .Property(cpu => cpu.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(cpu => cpu.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(cpu => cpu.Description)
        .HasMaxLength(500);

        builder
        .Property(cpu => cpu.Count)
        .IsRequired();

        builder
        .Property(cpu => cpu.Price)
        .IsRequired();

        builder.Property(cpu => cpu.Socket).IsRequired().HasMaxLength(10);
    }
}