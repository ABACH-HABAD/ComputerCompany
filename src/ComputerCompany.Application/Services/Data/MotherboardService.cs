using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class MotherboardService(IValidator<MotherboardModel> validator, IMotherboardRepository repository) :
BaseDataService<MotherboardModel, IMotherboardRepository>(validator, repository, "материнскую плату"),
IMotherboardService;