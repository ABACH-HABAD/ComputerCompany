using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Abstractions.Services.Data;
using ComputerCompany.Application.Abstractions.Services.Validation;
using ComputerCompany.Application.Results;
using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;

using ComputerCompany.Application.Client.BusinessModels;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class BuildViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private AssemblyModel _assembly = null!;

    private bool _consultationMode;
    private bool _iKnowWhatIWantMode;

    private double _desiredCost;
    private string _wishes = string.Empty;

    public bool ConsultationMode
    {
        get => _consultationMode;
        set
        {
            ChangeProperty(ref _consultationMode, value);
        }
    }

    public bool IKnowWhatIWantMode
    {
        get => _iKnowWhatIWantMode;
        set
        {
            ChangeProperty(ref _iKnowWhatIWantMode, value);
        }
    }

    public double DesiredCost
    {
        get => _desiredCost;
        set
        {
            ChangeProperty(ref _desiredCost, value);
        }
    }

    public string Wishes
    {
        get => _wishes;
        set
        {
            ChangeProperty(ref _wishes, value);
        }
    }

    public AssemblyModel Assembly
    {
        get => _assembly;
        private set
        {
            ChangeProperty(ref _assembly, value);
        }
    }

    public ICommand OrderAssemblyCommand { get; }

    public BuildViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        OrderAssemblyCommand = new AsyncRelayCommand(OrderAssemblyAsync);
    }

    private static T? FindComponentInShoppingCart<T>(IShoppingCartService shoppingCartService) where T : BaseComponentModel
    {
        IEnumerable<T> components = shoppingCartService.FindByType<T>();
        return components.FirstOrDefault();
    }

    private static AssemblyModel FindAssymblyInShoppingCart(IShoppingCartService shoppingCartService)
    {
        CpuModel? cpu = FindComponentInShoppingCart<CpuModel>(shoppingCartService);
        GpuModel? gpu = FindComponentInShoppingCart<GpuModel>(shoppingCartService);
        MotherboardModel? motherboard = FindComponentInShoppingCart<MotherboardModel>(shoppingCartService);
        MemoryModel? memory = FindComponentInShoppingCart<MemoryModel>(shoppingCartService);
        StorageModel? storage = FindComponentInShoppingCart<StorageModel>(shoppingCartService);
        PowerUnitModel? powerUnit = FindComponentInShoppingCart<PowerUnitModel>(shoppingCartService);
        FrameModel? frame = FindComponentInShoppingCart<FrameModel>(shoppingCartService);

        int memoryCount = shoppingCartService.FindCountByType<MemoryModel>();

        AssemblyModel formedAssembly = new
        (
            Id: default,
            Cpu: cpu!,
            Gpu: gpu!,
            Motherboard: motherboard!,
            Memory: memory!,
            Storage: storage!,
            PowerUnit: powerUnit!,
            Frame: frame!,
            UsedMemoryCount: memoryCount,
            Account: default!
        );

        return formedAssembly;
    }

    private static void AddAssemblyToShoppingCart(IShoppingCartService shoppingCartService)
    {
        if (!shoppingCartService.InPurchasesList(IShoppingCartService.PcAssemblyServiceName))
        {
            ServiceModel assemblyServiceModel = new
            (
                Id: default,
                Name: IShoppingCartService.PcAssemblyServiceName,
                Description: IShoppingCartService.PcAssemblyServiceDescription,
                Price: IShoppingCartService.PcAssemblyServicePrice
            );

            shoppingCartService.AddToShoppingCart(assemblyServiceModel);
        }
    }

    private static void AddConsultationToShoppingCart(IShoppingCartService shoppingCartService)
    {

        if (!shoppingCartService.InPurchasesList(IShoppingCartService.PcConsultationServiceName))
        {
            ServiceModel сonsultationServiceModel = new
            (
                Id: default,
                Name: IShoppingCartService.PcConsultationServiceName,
                Description: IShoppingCartService.PcConsultationServiceDescription,
                Price: IShoppingCartService.PcConsultationServicePrice
            );

            shoppingCartService.AddToShoppingCart(сonsultationServiceModel);
        }
    }

    private async Task OrderAssemblyAsync()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IShoppingCartService shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

        //Если выбран режим "Я знаю чего хочу" пользователь должен сам выбрать необходимые комплектующие
        if (IKnowWhatIWantMode)
        {
            AssemblyModel assembly;
            try
            {
                assembly = FindAssymblyInShoppingCart(shoppingCartService);

                IAsseblyCheckerService asseblyCheckerService = scope.ServiceProvider.GetRequiredService<IAsseblyCheckerService>();

                Result assemblyCheckResult;

                assemblyCheckResult = asseblyCheckerService.CheckAssemblyCompleteness(assembly);
                if (!assemblyCheckResult.IsSuccess)
                {
                    messageService.ShowErrorMessage(assemblyCheckResult.Message);
                    return;
                }

                assemblyCheckResult = asseblyCheckerService.CheckErrorsInAssembly(assembly);
                if (!assemblyCheckResult.IsSuccess)
                {
                    messageService.ShowErrorMessage(assemblyCheckResult.Message);
                    return;
                }

            }
            catch (Exception ex)
            {
                messageService.ShowErrorMessage(ex.Message);
                return;
            }

            IAssemblyService assemblyService = scope.ServiceProvider.GetRequiredService<IAssemblyService>();

            //Не забудем сохранить данные о сборке в бд
            Result serviceResult = await assemblyService.AddAsync(assembly);
            if (!serviceResult.IsSuccess)
            {
                messageService.ShowErrorMessage(serviceResult.Message);
                return;
            }
        }

        //Если же выбран режим консультации то необходимо указать желаемую цену и пожелания
        else if (ConsultationMode)
        {
            if (DesiredCost <= 0)
            {
                messageService.ShowErrorMessage("Укажите желаемую стоимость");
                return;
            }

            if (Wishes == null || Wishes == string.Empty)
            {
                messageService.ShowErrorMessage("Укажите выши пожелания");
                return;
            }

            AddConsultationToShoppingCart(shoppingCartService);
        }

        AddAssemblyToShoppingCart(shoppingCartService);

        messageService.ShowInformationMessage("Сборка ПК успешно заказана! Осталось только оплатить");
    }
}