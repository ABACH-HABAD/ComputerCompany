using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Validation;

public class AssemblyCheckerService : IAsseblyCheckerService
{
    public Result CheckAssemblyCompleteness(AssemblyModel assembly)
    {
        LinkedList<string> errors = [];

        if (assembly.Cpu == null) errors.AddLast("Отсутствует процессор\n");
        if (assembly.Gpu == null) errors.AddLast("Отсутствует видеокарта\n");
        if (assembly.Motherboard == null) errors.AddLast("Отсутствует материнская плата\n");
        if (assembly.Memory == null) errors.AddLast("Отсутствует оперативная память\n");
        if (assembly.Storage == null) errors.AddLast("Отсутствует накопитель\n");
        if (assembly.PowerUnit == null) errors.AddLast("Отсутствует блок питания\n");
        if (assembly.Frame == null) errors.AddLast("Отсутствует корпус\n");

        if (errors.Count > 0) return Result.Fail(errors);
        else return Result.Success();
    }

    public Result CheckErrorsInAssembly(AssemblyModel assembly)
    {
        LinkedList<string> errors = [];

        if (assembly.Cpu.Socket != assembly.Motherboard.Socket) errors.AddLast("Сокет процессора и материнской платы отличаются\n");
        if (assembly.Frame.FormFactor != assembly.PowerUnit.FormFactor) errors.AddLast("Формфактор блока питания отличается от формфактора корпуса\n");
        if (assembly.UsedMemoryCount < 0 || assembly.UsedMemoryCount % 2 != 0 || assembly.UsedMemoryCount > 8) errors.AddLast("Некорректное количество использованной оперативной памяти\n");

        if (errors.Count > 0) return Result.Fail(errors);
        else return Result.Success();
    }
}