using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientCpuService(IValidator<CpuModel> validator, IApiClientService httpClient) : 
BaseClientService<CpuModel>("cpu", validator, httpClient),
ICpuService;