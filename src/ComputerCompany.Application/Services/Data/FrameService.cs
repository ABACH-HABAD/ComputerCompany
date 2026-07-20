using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Core.Abstractions.Repositories;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Services.Data;

public class FrameService(IValidator<FrameModel> validator, IFrameRepository repository) :
BaseDataService<FrameModel, IFrameRepository>(validator, repository, "корпус"),
IFrameService;