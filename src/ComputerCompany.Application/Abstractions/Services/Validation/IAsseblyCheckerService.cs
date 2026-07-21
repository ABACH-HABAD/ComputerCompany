using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Abstractions.Services.Validation;

public interface IAsseblyCheckerService
{
    public Result CheckAssemblyCompleteness(AssemblyModel assembly);
    public Result CheckErrorsInAssembly(AssemblyModel assembly);
}