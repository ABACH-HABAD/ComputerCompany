using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Application.Abstractions.Services;

namespace ComputerCompany.Presentation.Services;

public class WindowsScopeFactory : IScopeFactory
{
    public IServiceScope CreateScope()
    {
        return App.Services.CreateScope();
    }
}