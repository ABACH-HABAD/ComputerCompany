using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientAssemblyService(IValidator<AssemblyModel> validator, IApiClientService httpClient) :
BaseClientService<AssemblyModel>("assembly", validator, httpClient),
IAssemblyService;