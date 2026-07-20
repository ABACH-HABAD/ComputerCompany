using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class GpuService(IValidator<GpuModel> validator, IGpuRepository repository) :
BaseDataService<GpuModel, IGpuRepository>(validator, repository, "видеокарту"),
IGpuService;