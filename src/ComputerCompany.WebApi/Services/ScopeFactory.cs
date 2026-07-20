using ComputerCompany.Application.Abstractions.Services;

namespace ComputerCompany.WebApi.Services;

public class ScopeFactory() : IScopeFactory
{
    public IServiceScope CreateScope()
    {
        return Program.ProgramServices.CreateScope();
    }
}