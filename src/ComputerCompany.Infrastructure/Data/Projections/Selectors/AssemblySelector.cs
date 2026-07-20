using System.Linq.Expressions;
using ComputerCompany.Core.Models;
using ComputerCompany.Infrastructure.Data.Entities;

namespace ComputerCompany.Infrastructure.Data.Projections.Selectors;

internal static class AssemblySelector
{
    internal static Expression<Func<AssemblyEntity, AssemblyModel>> ToAssemblyModel = assembly =>
    new AssemblyModel
    (
        assembly.Id,
        new CpuModel
        (
            assembly.Cpu.Id,
            assembly.Cpu.Name,
            assembly.Cpu.Description,
            assembly.Cpu.Price,
            assembly.Cpu.Count,
            assembly.Cpu.Model,
            assembly.Cpu.Socket
        ),
        new GpuModel
        (
            assembly.Gpu.Id,
            assembly.Gpu.Name,
            assembly.Gpu.Description,
            assembly.Gpu.Price,
            assembly.Gpu.Count,
            assembly.Gpu.Model,
            assembly.Gpu.ModelCore,
            assembly.Gpu.VideoMemory
        ),
        new MotherboardModel
        (
            assembly.Motherboard.Id,
            assembly.Motherboard.Name,
            assembly.Motherboard.Description,
            assembly.Motherboard.Price,
            assembly.Motherboard.Count,
            assembly.Motherboard.Model,
            assembly.Motherboard.Chipset,
            assembly.Motherboard.Socket
        ),
        new MemoryModel
        (
            assembly.Memory.Id,
            assembly.Memory.Name,
            assembly.Memory.Description,
            assembly.Memory.Price,
            assembly.Memory.Count,
            assembly.Memory.Model,
            assembly.Memory.Type,
            assembly.Memory.Size,
            assembly.Memory.Frequency
        ),
        new PowerUnitModel
        (
            assembly.PowerUnit.Id,
            assembly.PowerUnit.Name,
            assembly.PowerUnit.Description,
            assembly.PowerUnit.Price,
            assembly.PowerUnit.Count,
            assembly.PowerUnit.Model,
            assembly.PowerUnit.Certification,
            assembly.PowerUnit.FormFactor,
            assembly.PowerUnit.Power
        ),
        new StorageModel
        (
            assembly.Storage.Id,
            assembly.Storage.Name,
            assembly.Storage.Description,
            assembly.Storage.Price,
            assembly.Storage.Count,
            assembly.Storage.Model,
            assembly.Storage.FormFactor,
            assembly.Storage.Type,
            assembly.Storage.Size
        ),
        new FrameModel
        (
            assembly.Frame.Id,
            assembly.Frame.Name,
            assembly.Frame.Description,
            assembly.Frame.Price,
            assembly.Frame.Count,
            assembly.Frame.Model,
            assembly.Frame.FormFactor
        ),
        assembly.UsedMemoryCount,
        new AccountModel
        (
            assembly.Account.Id,
            assembly.Account.Login,
            assembly.Account.Name,
            new RoleModel
            (
                assembly.Account.Role.Id,
                assembly.Account.Role.Name
            )
        )
    );
}