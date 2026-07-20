using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class MemoryConfig : IEntityTypeConfiguration<MemoryEntity>
{

    public void Configure(EntityTypeBuilder<MemoryEntity> builder)
    {
        builder.ToTable("Memories").HasKey(memory => memory.Id);

        builder.HasIndex(memory => memory.Id).IsUnique();
        builder.HasIndex(memory => memory.Name).IsUnique();
        builder.HasIndex(memory => memory.Model).IsUnique();

        builder
        .Property(memory => memory.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(memory => memory.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(memory => memory.Description)
        .HasMaxLength(500);

        builder
        .Property(memory => memory.Count)
        .IsRequired();

        builder
        .Property(memory => memory.Price)
        .IsRequired();

        builder.Property(memory => memory.Type).IsRequired().HasMaxLength(5);
        builder.Property(memory => memory.Size).IsRequired();
        builder.Property(memory => memory.Frequency).IsRequired();
    }
}