namespace ComputerCompany.Core.Models;

public record AssemblyModel
(
    Guid Id,
    CpuModel Cpu,
    GpuModel Gpu,
    MotherboardModel Motherboard,
    MemoryModel Memory,
    PowerUnitModel PowerUnit,
    StorageModel Storage,
    FrameModel Frame,
    int UsedMemoryCount,
    AccountModel Account
) : BaseModel(Id);