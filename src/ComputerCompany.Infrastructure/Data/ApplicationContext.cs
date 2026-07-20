using ComputerCompany.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ComputerCompany.Infrastructure.Data;

public class ApplicationContext(DataBaseConnectionString dataBaseConnectionString) : DbContext
{
    internal DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    internal DbSet<SessionEntity> Sessions => Set<SessionEntity>();
    internal DbSet<RoleEntity> Roles => Set<RoleEntity>();
    internal DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    internal DbSet<AssemblyEntity> Assemblies => Set<AssemblyEntity>();

    internal DbSet<CpuEntity> Cpus => Set<CpuEntity>();
    internal DbSet<GpuEntity> Gpus => Set<GpuEntity>();
    internal DbSet<MotherboardEntity> Motherboards => Set<MotherboardEntity>();
    internal DbSet<MemoryEntity> Memories => Set<MemoryEntity>();
    internal DbSet<StorageEntity> Storages => Set<StorageEntity>();
    internal DbSet<PowerUnitEntity> PowerUnits => Set<PowerUnitEntity>();
    internal DbSet<FrameEntity> Frames => Set<FrameEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(dataBaseConnectionString.ConnectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SessionEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyEntity).Assembly);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CpuEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GpuEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MotherboardEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemoryEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StorageEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PowerUnitEntity).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FrameEntity).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}