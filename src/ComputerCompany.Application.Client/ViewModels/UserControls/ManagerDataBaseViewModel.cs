using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Results;
using ComputerCompany.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class ManagerDataBaseViewModel : BaseUserControlViewModel
{
    private const string Cpu = "Процессор";
    private const string Gpu = "Видеокарта";
    private const string Motherboard = "Материнская плата";
    private const string Memory = "Оперативная память";
    private const string Storage = "Накопитель";
    private const string PowerUnit = "Блок питания";
    private const string Frame = "Корпус";

    private readonly IScopeFactory _scopeFactory;

    private readonly ObservableCollection<string> _componentsTypes =
    [
        Cpu,
        Gpu,
        Motherboard,
        Memory,
        Storage,
        PowerUnit,
        Frame
    ];

    private bool _isCreatingComponent;
    private bool _isUpdatingComponent;
    private bool _isDeletingComponent;

    private string _selectedComponentType = string.Empty;

    public ObservableCollection<string> ComponentsTypes => _componentsTypes;

    public string SelectedComponentType
    {
        get => _selectedComponentType;
        set
        {
            ChangeProperty(ref _selectedComponentType, value);
            LoadDataCommand.Execute(value);
        }
    }

    public bool IsCreatinComponent
    {
        get => _isCreatingComponent;
        set => ChangeProperty(ref _isCreatingComponent, value);
    }

    public bool IsUpdatingComponent
    {
        get => _isUpdatingComponent;
        set => ChangeProperty(ref _isUpdatingComponent, value);

    }

    public bool IsDeletingComponent
    {
        get => _isDeletingComponent;
        set => ChangeProperty(ref _isDeletingComponent, value);
    }

    public ObservableCollection<BaseComponentModel> ComponentsTable { get; } = [];

    public ICommand LoadDataCommand { get; }
    public ICommand CreateComponentCommand { get; }
    public ICommand EditComponentCommand { get; }
    public ICommand DeleteComponentCommand { get; }

    public ManagerDataBaseViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        LoadDataCommand = new AsyncRelayCommand<string>(LoadDataAsync);
        CreateComponentCommand = new AsyncRelayCommand(CreateComponentAsync, CanCreateComponent);
        EditComponentCommand = new AsyncRelayCommand<BaseComponentModel>(UpdateComponentAsync, CanUpdateComponent);
        DeleteComponentCommand = new AsyncRelayCommand<BaseComponentModel>(DeleteComponentAsync, CanDeleteComponent);
    }

    private async Task LoadDataAsync<T>(IDataService<T> service) where T : BaseComponentModel
    {
        DataResult<List<T>> result = await service.GetAllAsync();
        if (!result.IsSuccess) return;

        ComponentsTable.Clear();

        foreach (T component in result.Data)
        {
            ComponentsTable.Add(component);
        }
    }

    private async Task CreateComponentAsync<T>(IDataService<T> service) where T : BaseComponentModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        DataResult<T> dialogResult = await dialogService.ShowCreateDialogAsync<T, ComponentDataBaseDialogViewModel<T>>($"Создание нового {SelectedComponentType}", [SelectedComponentType]);
        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        Result serviceResult = await service.AddAsync(dialogResult.Data);
        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }
    }

    private async Task UpdateComponentAsync<T>(IDataService<T> service, T component) where T : BaseComponentModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        DataResult<T> dialogResult = await dialogService.ShowUpdateDialogAsync<T, ComponentDataBaseDialogViewModel<T>>(component, $"Редактирование {SelectedComponentType} {component.Name}", [SelectedComponentType]);
        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        Result serviceResult = await service.UpdateAsync(dialogResult.Data);
        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }
    }

    private async Task DeleteComponentAsync<T>(IDataService<T> service, T component) where T : BaseComponentModel
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

        Result dialogResult = await dialogService.ShowDeleteDialogAsync($"Удаление {SelectedComponentType} {component.Name}", $"Вы уверены что хотите удалть {SelectedComponentType} {component.Name}?\nЭто действие нельзя будет отменить");
        if (!dialogResult.IsSuccess)
        {
            if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            return;
        }

        Result serviceResult = await service.DeleteAsync(component);
        if (!serviceResult.IsSuccess)
        {
            messageService.ShowErrorMessage(serviceResult.Message);
            return;
        }
    }

    private async Task LoadDataAsync(string type)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        switch (type)
        {
            case Cpu:
                ICpuService cpuService = scope.ServiceProvider.GetRequiredService<ICpuService>();
                await LoadDataAsync(cpuService);
                break;
            case Gpu:
                IGpuService gpuService = scope.ServiceProvider.GetRequiredService<IGpuService>();
                await LoadDataAsync(gpuService);
                break;
            case Motherboard:
                IMotherboardService motherboardService = scope.ServiceProvider.GetRequiredService<IMotherboardService>();
                await LoadDataAsync(motherboardService);
                break;
            case Memory:
                IMemoryService memoryService = scope.ServiceProvider.GetRequiredService<IMemoryService>();
                await LoadDataAsync(memoryService);
                break;
            case Storage:
                IStorageService storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
                await LoadDataAsync(storageService);
                break;
            case PowerUnit:
                IPowerUnitService powerUnitService = scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
                await LoadDataAsync(powerUnitService);
                break;
            case Frame:
                IFrameService frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                await LoadDataAsync(frameService);
                break;
            default: throw new Exception("Такого типа не существует");
        }
    }

    private async Task CreateComponentAsync()
    {
        IsCreatinComponent = true;
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            if (SelectedComponentType == null || SelectedComponentType == string.Empty)
            {
                IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                messageService.ShowErrorMessage("Укажите тип компонента");
                return;
            }

            switch (SelectedComponentType)
            {
                case Cpu:
                    ICpuService cpuService = scope.ServiceProvider.GetRequiredService<ICpuService>();
                    await CreateComponentAsync(cpuService);
                    break;
                case Gpu:
                    IGpuService gpuService = scope.ServiceProvider.GetRequiredService<IGpuService>();
                    await CreateComponentAsync(gpuService);
                    break;
                case Motherboard:
                    IMotherboardService motherboardService = scope.ServiceProvider.GetRequiredService<IMotherboardService>();
                    await CreateComponentAsync(motherboardService);
                    break;
                case Memory:
                    IMemoryService memoryService = scope.ServiceProvider.GetRequiredService<IMemoryService>();
                    await CreateComponentAsync(memoryService);
                    break;
                case Storage:
                    IStorageService storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
                    await CreateComponentAsync(storageService);
                    break;
                case PowerUnit:
                    IPowerUnitService powerUnitService = scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
                    await CreateComponentAsync(powerUnitService);
                    break;
                case Frame:
                    IFrameService frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                    await CreateComponentAsync(frameService);
                    break;
                default:
                    throw new Exception("Такого типа не существует");
            }
        }
        finally
        {
            if (SelectedComponentType != null && SelectedComponentType != string.Empty) LoadDataCommand.Execute(SelectedComponentType);
            IsCreatinComponent = false;
        }
    }

    private async Task UpdateComponentAsync(BaseComponentModel component)
    {
        IsUpdatingComponent = true;
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            if (SelectedComponentType == null || SelectedComponentType == string.Empty)
            {
                IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                messageService.ShowErrorMessage("Укажите тип компонента");
                return;
            }

            switch (SelectedComponentType)
            {
                case Cpu:
                    ICpuService cpuService = scope.ServiceProvider.GetRequiredService<ICpuService>();
                    await UpdateComponentAsync(cpuService, (component as CpuModel)!);
                    break;
                case Gpu:
                    IGpuService gpuService = scope.ServiceProvider.GetRequiredService<IGpuService>();
                    await UpdateComponentAsync(gpuService, (component as GpuModel)!);
                    break;
                case Motherboard:
                    IMotherboardService motherboardService = scope.ServiceProvider.GetRequiredService<IMotherboardService>();
                    await UpdateComponentAsync(motherboardService, (component as MotherboardModel)!);
                    break;
                case Memory:
                    IMemoryService memoryService = scope.ServiceProvider.GetRequiredService<IMemoryService>();
                    await UpdateComponentAsync(memoryService, (component as MemoryModel)!);
                    break;
                case Storage:
                    IStorageService storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
                    await UpdateComponentAsync(storageService, (component as StorageModel)!);
                    break;
                case PowerUnit:
                    IPowerUnitService powerUnitService = scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
                    await UpdateComponentAsync(powerUnitService, (component as PowerUnitModel)!);
                    break;
                case Frame:
                    IFrameService frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                    await UpdateComponentAsync(frameService, (component as FrameModel)!);
                    break;
                default:
                    throw new Exception("Такого типа не существует");
            }
        }
        finally
        {
            if (SelectedComponentType != null && SelectedComponentType != string.Empty) LoadDataCommand.Execute(SelectedComponentType);
            IsUpdatingComponent = false;
        }
    }

    private async Task DeleteComponentAsync(BaseComponentModel component)
    {
        IsDeletingComponent = true;
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            if (SelectedComponentType == null || SelectedComponentType == string.Empty)
            {
                IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                messageService.ShowErrorMessage("Укажите тип компонента");
                return;
            }

            switch (SelectedComponentType)
            {
                case Cpu:
                    ICpuService cpuService = scope.ServiceProvider.GetRequiredService<ICpuService>();
                    await DeleteComponentAsync(cpuService, (component as CpuModel)!);
                    break;
                case Gpu:
                    IGpuService gpuService = scope.ServiceProvider.GetRequiredService<IGpuService>();
                    await DeleteComponentAsync(gpuService, (component as GpuModel)!);
                    break;
                case Motherboard:
                    IMotherboardService motherboardService = scope.ServiceProvider.GetRequiredService<IMotherboardService>();
                    await DeleteComponentAsync(motherboardService, (component as MotherboardModel)!);
                    break;
                case Memory:
                    IMemoryService memoryService = scope.ServiceProvider.GetRequiredService<IMemoryService>();
                    await DeleteComponentAsync(memoryService, (component as MemoryModel)!);
                    break;
                case Storage:
                    IStorageService storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
                    await DeleteComponentAsync(storageService, (component as StorageModel)!);
                    break;
                case PowerUnit:
                    IPowerUnitService powerUnitService = scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
                    await DeleteComponentAsync(powerUnitService, (component as PowerUnitModel)!);
                    break;
                case Frame:
                    IFrameService frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                    await DeleteComponentAsync(frameService, (component as FrameModel)!);
                    break;
                default:
                    throw new Exception("Такого типа не существует");
            }
        }
        finally
        {
            if (SelectedComponentType != null && SelectedComponentType != string.Empty) LoadDataCommand.Execute(SelectedComponentType);
            IsDeletingComponent = false;
        }
    }

    private bool CanCreateComponent() => !IsCreatinComponent;
    private bool CanUpdateComponent(BaseComponentModel? baseComponentModel) => !IsUpdatingComponent;
    private bool CanDeleteComponent(BaseComponentModel? baseComponentModel) => !IsDeletingComponent;
}