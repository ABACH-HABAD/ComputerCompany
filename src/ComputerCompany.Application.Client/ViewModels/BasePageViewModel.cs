using ComputerCompany.Application.Client.Abstractions.ViewModels;

namespace ComputerCompany.Application.Client.ViewModels;

public abstract class BaseUserControlViewModel : BaseViewModel, IUserControlViewModel
{
    public virtual void OnNavigatedTo() { }
    public virtual void OnNavigatedFrom() { }
}