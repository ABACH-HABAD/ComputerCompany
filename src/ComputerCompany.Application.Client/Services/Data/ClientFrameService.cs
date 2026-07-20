using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientFrameService(IValidator<FrameModel> validator, IApiClientService httpClient) :
BaseClientService<FrameModel>("frame", validator, httpClient),
IFrameService;