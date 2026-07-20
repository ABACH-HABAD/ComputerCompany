using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientPowerUnitService(IValidator<PowerUnitModel> validator, IApiClientService httpClient) :
BaseClientService<PowerUnitModel>("powerunit", validator, httpClient),
IPowerUnitService;