using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

public interface IWindowService
{
    public void ShowWindow<TViewModel>() where TViewModel : BaseViewModel, IWindowViewModel;

    /// <summary>
    /// Закрывает окно, которому соответствует ViewModel.  
    /// Будет работать ТОЛЬКО с ViewModel, время жизни которых Singleton  
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel вашего окна</typeparam>
    public void CloseWindow<TViewModel>() where TViewModel : BaseViewModel, IWindowViewModel;
}