using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class AssemblyService(IValidator<AssemblyModel> validator, IAssemblyRepository repository) :
BaseDataService<AssemblyModel, IAssemblyRepository>(validator, repository, "сборку"),
IAssemblyService;