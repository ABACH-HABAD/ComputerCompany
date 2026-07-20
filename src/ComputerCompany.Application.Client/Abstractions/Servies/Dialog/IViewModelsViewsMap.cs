using ComputerCompany.Application.Client.Abstractions.ViewModels;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

public interface IViewModelsViewsMap
{
    public Type GetViewType(Type viewModelType);
    public Type GetViewType<T>() where T : IViewModel;
}