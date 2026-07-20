namespace ComputerCompany.Infrastructure.Data.Entities;

internal class AssemblyEntity : BaseEntity
{
    public int UsedMemoryCount { get; set; }

    public Guid AccountId { get; set; }

    public Guid CpuId { get; set; }
    public Guid MotherboardId { get; set; }
    public Guid MemoryId { get; set; }
    public Guid PowerUnitId { get; set; }
    public Guid StorageId { get; set; }
    public Guid FrameId { get; set; }
    public Guid GpuId { get; set; }

    public AccountEntity Account { get; set; } = null!;

    public CpuEntity Cpu { get; set; } = null!;
    public MotherboardEntity Motherboard { get; set; } = null!;
    public MemoryEntity Memory { get; set; } = null!;
    public PowerUnitEntity PowerUnit { get; set; } = null!;
    public StorageEntity Storage { get; set; } = null!;
    public FrameEntity Frame { get; set; } = null!;
    public GpuEntity Gpu { get; set; } = null!;
}