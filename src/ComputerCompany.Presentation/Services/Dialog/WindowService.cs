using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;

namespace ComputerCompany.Presentation.Services.Dialog;

public class WindowService(IScopeFactory scopeFactory, IViewModelsViewsMap viewModelsViewsMap) : IWindowService
{
    private readonly IScopeFactory _scopeFactory = scopeFactory;
    private readonly IViewModelsViewsMap _viewModelsViewsMap = viewModelsViewsMap;

    public void CloseWindow<TViewModel>() where TViewModel : BaseViewModel, IWindowViewModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        //Получаем нужный ViewModel
        TViewModel viewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();

        //Вызываем событие для закрытия окна
        viewModel.Close();
    }

    public void ShowWindow<TViewModel>() where TViewModel : BaseViewModel, IWindowViewModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        //Находим подходящее окно
        Type WindowType = _viewModelsViewsMap.GetViewType<TViewModel>();

        //Создаём его
        object? service = scope.ServiceProvider.GetService(WindowType);
        if (service is not Window window) throw new InvalidOperationException($"Несуществует подходящего окна для {nameof(TViewModel)}");

        //Получаем нужный ViewModel
        TViewModel viewModel;
        try
        {
            viewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка: " + ex.Message, innerException: ex);
        }

        //Подписываемся на событие для закрытия окна
        viewModel.Closed += (s, e) =>
        {
            window.Close();
        };

        //Показываем окно
        window.DataContext = viewModel;
        window.Show();
    }
}