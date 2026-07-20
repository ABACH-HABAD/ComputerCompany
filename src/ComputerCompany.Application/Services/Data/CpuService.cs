using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class CpuService(IValidator<CpuModel> validator, ICpuRepository repository) :
BaseDataService<CpuModel, ICpuRepository>(validator, repository, "процессор"),
ICpuService;