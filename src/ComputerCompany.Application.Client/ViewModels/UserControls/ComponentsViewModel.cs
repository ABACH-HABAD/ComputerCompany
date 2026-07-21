using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class ComponentsViewModel : BaseUserControlViewModel
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

    public ObservableCollection<string> ComponentsTypes => _componentsTypes;

    private string _selectedComponentType = string.Empty;
    public string SelectedComponentType
    {
        get => _selectedComponentType;
        set
        {
            ChangeProperty(ref _selectedComponentType, value);
            UpdateDataCommand.Execute(value);
        }
    }

    public ICommand UpdateDataCommand { get; }
    public ICommand AddToBasketCommand { get; }

    public ObservableCollection<BaseComponentModel> ComponentsTable { get; } = [];

    public ComponentsViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        UpdateDataCommand = new AsyncRelayCommand<string>(UpdateDataAsync);
        AddToBasketCommand = new RelayCommand<BaseComponentModel>(AddToShoppingCart);
    }

    private async Task UpdateDataAsync<T>(IDataService<T> service) where T : BaseComponentModel
    {
        DataResult<List<T>> result = await service.GetAllAsync();
        if (!result.IsSuccess) return;

        ComponentsTable.Clear();

        foreach (T component in result.Data)
        {
            ComponentsTable.Add(component);
        }
    }

    private async Task UpdateDataAsync(string type)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        switch (type)
        {
            case Cpu:
                ICpuService cpuService = scope.ServiceProvider.GetRequiredService<ICpuService>();
                await UpdateDataAsync(cpuService);
                break;
            case Gpu:
                IGpuService gpuService = scope.ServiceProvider.GetRequiredService<IGpuService>();
                await UpdateDataAsync(gpuService);
                break;
            case Motherboard:
                IMotherboardService motherboardService = scope.ServiceProvider.GetRequiredService<IMotherboardService>();
                await UpdateDataAsync(motherboardService);
                break;
            case Memory:
                IMemoryService memoryService = scope.ServiceProvider.GetRequiredService<IMemoryService>();
                await UpdateDataAsync(memoryService);
                break;
            case Storage:
                IStorageService storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
                await UpdateDataAsync(storageService);
                break;
            case PowerUnit:
                IPowerUnitService powerUnitService = scope.ServiceProvider.GetRequiredService<IPowerUnitService>();
                await UpdateDataAsync(powerUnitService);
                break;
            case Frame:
                IFrameService frameService = scope.ServiceProvider.GetRequiredService<IFrameService>();
                await UpdateDataAsync(frameService);
                break;
            default: throw new Exception("Такого типа не существует");
        }
    }

    private void AddToShoppingCart(BaseComponentModel component)
    {
        using IServiceScope serviceScope = _scopeFactory.CreateScope();
        IShoppingCartService shoppingCartService = serviceScope.ServiceProvider.GetRequiredService<IShoppingCartService>();
        IMessageService messageService = serviceScope.ServiceProvider.GetRequiredService<IMessageService>();

        shoppingCartService.AddToShoppingCart(component);
        messageService.ShowInformationMessage($"{component.Name} добавлен в корзину");
    }
}