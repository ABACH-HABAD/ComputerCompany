using Microsoft.Extensions.DependencyInjection;

namespace ComputerCompany.Application.Abstractions.Services;

public interface IScopeFactory
{
    public IServiceScope CreateScope();
}