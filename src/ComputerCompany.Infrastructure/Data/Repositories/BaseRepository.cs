namespace ComputerCompany.Infrastructure.Data.Repositories;

public abstract class BaseRepository(ApplicationContext applicationContext)
{
    protected ApplicationContext _applicationContext = applicationContext;
}