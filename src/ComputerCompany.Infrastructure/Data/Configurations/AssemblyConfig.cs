using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Configurations;

internal class AssemblyConfig : IEntityTypeConfiguration<AssemblyEntity>
{

    public void Configure(EntityTypeBuilder<AssemblyEntity> builder)
    {
        builder.ToTable("Assemblies").HasKey(assembly => assembly.Id);

        builder.HasIndex(assembly => assembly.Id).IsUnique();

        builder.Property(assembly => assembly.UsedMemoryCount).IsRequired();

        builder.HasOne(assembly => assembly.Cpu).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.CpuId);
        builder.HasOne(assembly => assembly.Gpu).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.GpuId);
        builder.HasOne(assembly => assembly.Motherboard).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.MotherboardId);
        builder.HasOne(assembly => assembly.Memory).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.MemoryId);
        builder.HasOne(assembly => assembly.Storage).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.StorageId);
        builder.HasOne(assembly => assembly.PowerUnit).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.PowerUnitId);
        builder.HasOne(assembly => assembly.Frame).WithMany(component => component.Assemblies).HasForeignKey(assembly => assembly.FrameId);

        builder.HasOne(assembly => assembly.Account).WithMany(account => account.Assemblies).HasForeignKey(assembly => assembly.AccountId);
    }
}