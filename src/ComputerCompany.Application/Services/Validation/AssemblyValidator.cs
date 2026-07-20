using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Validation;

public class AssemblyValidator
(
    IValidator<CpuModel> cpuValidator,
    IValidator<GpuModel> gpuValidator,
    IValidator<MemoryModel> memoryValidator,
    IValidator<MotherboardModel> motherboardValidator,
    IValidator<StorageModel> storageValidator,
    IValidator<PowerUnitModel> powerUnitValidator,
    IValidator<FrameModel> frameValidator,
    IValidator<AccountModel> accountValidator
) : IValidator<AssemblyModel>
{
    public Result Validate(AssemblyModel data)
    {
        if (data == null) return Result.Fail("Сборка" + ValidationMessages.Absent);

        LinkedList<string> errors = [];

        Result result;

        result = NumberValidator.ValidateNumber(data.UsedMemoryCount, "Количество оперативной памяти");
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = cpuValidator.Validate(data.Cpu);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = gpuValidator.Validate(data.Gpu);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = memoryValidator.Validate(data.Memory);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = storageValidator.Validate(data.Storage);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = motherboardValidator.Validate(data.Motherboard);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = powerUnitValidator.Validate(data.PowerUnit);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = frameValidator.Validate(data.Frame);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        result = accountValidator.Validate(data.Account);
        if (!result.IsSuccess) errors.AddLast(result.Message);

        if (errors.Count > 0) return Result.Fail([.. errors]);
        else return Result.Success();
    }
}