using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

public interface IDialogService
{
    public Task<Result> ShowInformationDialogAsync<TViewModel>
    (
        string? title = null,
        params object[]? parametrs
    )
    where TViewModel : BaseViewModel, IWindowViewModel;

    public Task<Result> ShowDataDialogAsync<TModel, TViewModel>
    (
        TModel? model = default,
        string? title = null,
        object[]? parametrs = null
    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel;

    public  Task<DataResult<TModel>> ShowCreateDialogAsync<TModel, TViewModel>
    (
        string? title = null,
        object[]? parametrs = null
    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel;

    public Task<DataResult<TModel>> ShowUpdateDialogAsync<TModel, TViewModel>
    (
        TModel model,
        string? title = null,
        object[]? parametrs = null
    )
    where TViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
    where TModel : BaseModel;

    public Task<Result> ShowDeleteDialogAsync(string title, string message);
}