using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientGpuService(IValidator<GpuModel> validator, IApiClientService httpClient) :
BaseClientService<GpuModel>("gpu", validator, httpClient),
IGpuService;