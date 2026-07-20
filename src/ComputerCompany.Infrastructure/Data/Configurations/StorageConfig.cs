using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class StorageConfig : IEntityTypeConfiguration<StorageEntity>
{

    public void Configure(EntityTypeBuilder<StorageEntity> builder)
    {
        builder.ToTable("Storages").HasKey(storage => storage.Id);

        builder.HasIndex(storage => storage.Id).IsUnique();
        builder.HasIndex(storage => storage.Name).IsUnique();
        builder.HasIndex(storage => storage.Model).IsUnique();

        builder
        .Property(storage => storage.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(storage => storage.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(storage => storage.Description)
        .HasMaxLength(500);

        builder
        .Property(storage => storage.Count)
        .IsRequired();

        builder
        .Property(storage => storage.Price)
        .IsRequired();

        builder.Property(storage => storage.Type).IsRequired().HasMaxLength(10);
        builder.Property(storage => storage.FormFactor).IsRequired().HasMaxLength(10);
        builder.Property(storage => storage.Size).IsRequired();
    }
}