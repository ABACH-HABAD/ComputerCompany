using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Client.Abstractions.Servies.Api;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.Services.Data;

public class ClientAssemblyService(IValidator<AssemblyModel> validator, IAccountService accountService, IApiClientService httpClient) :
BaseClientService<AssemblyModel>("assembly", validator, httpClient),
IAssemblyService
{
    private readonly IAccountService _accountService = accountService;

    public override async Task<Result> AddAsync(AssemblyModel data, CancellationToken cancellationToken = default)
    {
        if (data.Account == null || data.Account.Id == Guid.Empty)
        {
            DataResult<AccountModel> serviceResult = await _accountService.GetByIdAsync(default, cancellationToken);
            if (serviceResult.IsSuccess) data = data with { Account = serviceResult.Data };
        }

        return await base.AddAsync(data, cancellationToken);
    }
}