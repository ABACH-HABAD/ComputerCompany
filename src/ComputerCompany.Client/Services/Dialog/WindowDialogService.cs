using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Presentation.Services.Dialog;

public class WindowDialogService(IScopeFactory scopeFactory, IViewModelsViewsMap viewModelsViewsMap) : IDialogService
{
    private readonly IScopeFactory _scopeFactory = scopeFactory;
    private readonly IViewModelsViewsMap _viewModelsViewsMap = viewModelsViewsMap;

    public async Task<Result> ShowInformationDialogAsync<TViewModel>
    (
        string? title = null,
        params object[]? parametrs
    )
    where TViewModel : BaseViewModel, IWindowViewModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        //Получаем нужный ViewModel
        TViewModel viewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();

        //Заполняем данными, если нужно
        if (viewModel is IDataRequireableViewModel requireableViewModel && parametrs != null)
        {
            requireableViewModel.LoadData(parametrs);
        }
        if (viewModel is IDialogModeViewModel modeViewModel)
        {
            modeViewModel.LoadDialogMode();
        }

        //Находим подходящее окно
        Type WindowType = _viewModelsViewsMap.GetViewType<TViewModel>();

        //Создаём его
        object? service = scope.ServiceProvider.GetService(WindowType);
        if (service is not Window window) throw new InvalidOperationException($"Несуществует подходящего окна для {nameof(TViewModel)}");

        //Задаём название
        if (title != null) window.Title = title;

        //Подписываемся на событие для закрытия окна
        viewModel.Closed += (s, e) =>
        {
            window.Close();
        };

        //Задаём контекст
        window.DataContext = viewModel;

        //Показываем окно
        window.ShowDialog();

        //Если диалог возвращает какие либо данные, отдаём их
        if (viewModel is IDialogViewModel dialogViewModel)
        {
            if (dialogViewModel.DialogResult)
            {
                if (dialogViewModel.ResultData != null)
                {
                    return DataResult<object>.Success(dialogViewModel.ResultData);
                }
                else return Result.Success();
            }
            else return Result.Fail();
        }

        //Но в большинстве случаев это информационный диалог, цель которого просто показать сообщение на экране. Возвращаем Success.
        else return Result.Success();
    }

    public async Task<Result> ShowDataDialogAsync<TModel, TViewModel>
    (
        TModel? model = default,
        string? title = null,
        object[]? parametrs = null

    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        //Получаем нужный ViewModel
        TViewModel viewModel = scope.ServiceProvider.GetRequiredService<TViewModel>();

        //Заполняем данными, если нужно
        if (viewModel is IDataRequireableViewModel requireableViewModel && parametrs != null)
        {
            requireableViewModel.LoadData(parametrs);
        }
        if (viewModel is IDataViewModel<TModel> dataViewModel && model != null)
        {
            dataViewModel.SetData(model);
        }
        if (viewModel is IDialogModeViewModel modeViewModel)
        {
            modeViewModel.LoadDialogMode();
        }

        //Находим подходящее окно
        Type WindowType = _viewModelsViewsMap.GetViewType<TViewModel>();

        //Создаём его
        object? service = scope.ServiceProvider.GetService(WindowType);
        if (service is not Window window) throw new InvalidOperationException($"Несуществует подходящего окна для {nameof(TViewModel)}");

        //Задаём название
        if (title != null) window.Title = title;

        //Подписываемся на событие для закрытия окна
        viewModel.Closed += (s, e) =>
        {
            window.Close();
        };

        //Задаём контекст
        window.DataContext = viewModel;

        //Показываем окно
        window.ShowDialog();

        if (viewModel.DialogResult)
        {
            if (viewModel is IDataViewModel<TModel> dataResultViewModel)
            {
                DataResult<TModel> dataResult = DataResult<TModel>.Success(dataResultViewModel.Data);
                return dataResult;
            }
            else if (viewModel.ResultData != null)
            {
                return DataResult<object>.Success(viewModel.ResultData); 
            }
            else return Result.Success();
        }
        else return Result.Fail();
    }

    public async Task<DataResult<TModel>> ShowCreateDialogAsync<TModel, TViewModel>
    (
        string? title = null,
        object[]? parametrs = null
    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel
    {
        Result result = await ShowDataDialogAsync<TModel, TViewModel>(title: title, parametrs: parametrs);

        if (result is DataResult<TModel> dataResult) return dataResult;
        else if (!result.IsSuccess) return DataResult<TModel>.Fail(result.Message);
        else throw new Exception("Дилог не вернул данные");
    }

    public async Task<DataResult<TModel>> ShowUpdateDialogAsync<TModel, TViewModel>
    (
        TModel model,
        string? title = null,
        object[]? parametrs = null
    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel
    {
        ArgumentNullException.ThrowIfNull(model);

        Result result = await ShowDataDialogAsync<TModel, TViewModel>(model: model, title: title, parametrs: parametrs);

        if (result is DataResult<TModel> dataResult) return dataResult;
        else if (!result.IsSuccess) return DataResult<TModel>.Fail(result.Message);
        else throw new Exception("Дилог не вернул данные");
    }

    public async Task<Result> ShowDeleteDialogAsync
    (
        string title,
        string message
    )
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMessageService _messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

        bool answer = _messageService.ShowQuestionMessage(message, title);

        if (answer) return Result.Success();
        else return Result.Fail();
    }
}