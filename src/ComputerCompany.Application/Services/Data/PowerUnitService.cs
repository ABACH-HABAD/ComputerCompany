using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class PowerUnitService(IValidator<PowerUnitModel> validator, IPowerUnitRepository repository) :
BaseDataService<PowerUnitModel, IPowerUnitRepository>(validator, repository, "блок питания"),
IPowerUnitService;