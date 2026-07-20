using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class GpuConfig : IEntityTypeConfiguration<GpuEntity>
{

    public void Configure(EntityTypeBuilder<GpuEntity> builder)
    {
        builder.ToTable("Gpus").HasKey(gpu => gpu.Id);

        builder.HasIndex(gpu => gpu.Id).IsUnique();
        builder.HasIndex(gpu => gpu.Name).IsUnique();
        builder.HasIndex(gpu => gpu.Model).IsUnique();

        builder
        .Property(gpu => gpu.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(gpu => gpu.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(gpu => gpu.Description)
        .HasMaxLength(500);

        builder
        .Property(gpu => gpu.Count)
        .IsRequired();

        builder
        .Property(gpu => gpu.Price)
        .IsRequired();

        builder.Property(gpu => gpu.ModelCore).IsRequired().HasMaxLength(30);
        builder.Property(gpu => gpu.VideoMemory).IsRequired();
    }
}