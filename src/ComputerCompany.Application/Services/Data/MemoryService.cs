using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class MemoryService(IValidator<MemoryModel> validator, IMemoryRepository repository) :
BaseDataService<MemoryModel, IMemoryRepository>(validator, repository, "память"),
IMemoryService;