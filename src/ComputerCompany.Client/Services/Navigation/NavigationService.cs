using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ComputerCompany.Presentation.Services.Navigation;

public class NavigationService(IScopeFactory scopeFactory) : INavigationService
{
    protected readonly IScopeFactory _scopeFactory = scopeFactory;

    protected object _currnetUserControl = null!;
    protected BaseUserControlViewModel _currentViewModel = null!;

    public object CurrentUserControl
    {
        get => _currnetUserControl;
        set => _currnetUserControl = value;
    }

    public BaseUserControlViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel?.OnNavigatedFrom();
            _currentViewModel = value;
            _currentViewModel.OnNavigatedTo();

            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void NavigateTo<TViewModel>() where TViewModel : BaseUserControlViewModel
    {
        if (CurrentViewModel is TViewModel) return;

        using IServiceScope scope = _scopeFactory.CreateScope();
        TViewModel UserControlViewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();

        FrameworkElement view = CreateViewForViewModel<TViewModel>(scope);
        view.DataContext = UserControlViewModel;
        CurrentUserControl = view;

        CurrentViewModel = UserControlViewModel;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TViewModel)));
    }

    public void NavigateTo<TViewModel>(INavigationOwnerViewModel navigationOwnerViewModel) where TViewModel : BaseUserControlViewModel
    {
        navigationOwnerViewModel.NavigationService.NavigateTo<TViewModel>();
    }

    private static FrameworkElement CreateViewForViewModel<TViewModel>(IServiceScope scope) where TViewModel : BaseUserControlViewModel
    {
        IViewModelsViewsMap viewModelsViewsMap = scope.ServiceProvider.GetRequiredService<IViewModelsViewsMap>();
        Type UserControlType = viewModelsViewsMap.GetViewType<TViewModel>();

        object? UserControlObject = scope.ServiceProvider.GetService(UserControlType);
        if (UserControlObject is not FrameworkElement element) throw new Exception("Страницы не существует");

        return element;
    }
}