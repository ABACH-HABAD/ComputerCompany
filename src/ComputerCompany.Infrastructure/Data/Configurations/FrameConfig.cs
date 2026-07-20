using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class FrameConfig : IEntityTypeConfiguration<FrameEntity>
{

    public void Configure(EntityTypeBuilder<FrameEntity> builder)
    {
        builder.ToTable("Frames").HasKey(frame => frame.Id);

        builder.HasIndex(frame => frame.Id).IsUnique();
        builder.HasIndex(frame => frame.Name).IsUnique();
        builder.HasIndex(frame => frame.Model).IsUnique();

        builder
        .Property(frame => frame.Name)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(frame => frame.Model)
        .IsRequired()
        .HasMaxLength(100);

        builder
        .Property(frame => frame.Description)
        .HasMaxLength(500);

        builder
        .Property(frame => frame.Count)
        .IsRequired();

        builder
        .Property(frame => frame.Price)
        .IsRequired();

        builder.Property(frame => frame.FormFactor).IsRequired().HasMaxLength(10);
    }
}