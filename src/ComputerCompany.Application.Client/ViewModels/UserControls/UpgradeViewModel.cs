using System.Diagnostics;
using System.Windows.Input;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;
using ComputerCompany.Application.Client.BusinessModels;
using ComputerCompany.Application.Client.Services.Shopping;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class UpgradeViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;
    private bool _consultationMode;
    private bool _iKnowWhatIWantMode;

    private string _cpu = string.Empty;
    private string _gpu = string.Empty;
    private string _motherboard = string.Empty;
    private string _memory = string.Empty;

    private string _preferences = string.Empty;

    private bool _isScaning;

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

    public bool IsScaning
    {
        get => _isScaning;
        set
        {
            ChangeProperty(ref _isScaning, value);
        }
    }

    public string Cpu
    {
        get => _cpu;
        set { ChangeProperty(ref _cpu, value); }
    }

    public string Gpu
    {
        get => _gpu;
        set { ChangeProperty(ref _gpu, value); }
    }

    public string Motherboard
    {
        get => _motherboard;
        set { ChangeProperty(ref _motherboard, value); }
    }

    public string Memory
    {
        get => _memory;
        set { ChangeProperty(ref _memory, value); }
    }

    public string Preferences
    {
        get => _preferences;
        set
        {
            ChangeProperty(ref _preferences, value);
        }
    }

    public ICommand AutoScanCommand { get; }
    public ICommand OrderUpgradeCommand { get; }

    public UpgradeViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        AutoScanCommand = new RelayCommand(AutoScan, CanScan);
        OrderUpgradeCommand = new RelayCommand(OrderUpgrade);
    }

    private void AutoScan()
    {
        IsScaning = true;
        Process scanner = Process.Start("dxdiag");
        scanner.Exited += (s, e) => IsScaning = false;
    }

    private static void AddUpgradeServiceToShoppingCart(IShoppingCartService shoppingCartService)
    {
        if (!shoppingCartService.InPurchasesList(IShoppingCartService.PcUpgradeServiceName))
        {
            ServiceModel upgradeService = new
            (
                Id: default,
                Name: IShoppingCartService.PcUpgradeServiceName,
                Description: IShoppingCartService.PcUpgradeServiceDescription,
                Price: IShoppingCartService.PcUpgradeServicePrice
            );

            shoppingCartService.AddToShoppingCart(upgradeService);
        }
    }

    private static void AddConsultationServiceToShoppingCart(IShoppingCartService shoppingCartService)
    {
        if (!shoppingCartService.InPurchasesList(IShoppingCartService.PcConsultationServiceName))
        {
            ServiceModel consultationService = new
            (
                Id: default,
                Name: IShoppingCartService.PcConsultationServiceName,
                Description: IShoppingCartService.PcConsultationServiceDescription,
                Price: IShoppingCartService.PcConsultationServicePrice
            );

            shoppingCartService.AddToShoppingCart(consultationService);
        }
    }

    private void OrderUpgrade()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
        IShoppingCartService shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

        if (Cpu == null || Cpu == string.Empty)
        {
            messageService.ShowErrorMessage("Укажите ваш текущий процессор", "Заполните характеристика вашего ПК");
            return;
        }

        if (Gpu == null || Gpu == string.Empty)
        {
            messageService.ShowErrorMessage("Укажите вашу текущую видеокарту", "Заполните характеристика вашего ПК");
            return;
        }

        if (Motherboard == null || Motherboard == string.Empty)
        {
            messageService.ShowErrorMessage("Укажите вашу текущую материнскую плату", "Заполните характеристика вашего ПК");
            return;
        }

        if (Memory == null || Memory == string.Empty)
        {
            messageService.ShowErrorMessage("Укажите ваш текущий объём памяти", "Заполните характеристика вашего ПК");
            return;
        }

        if (ConsultationMode)
        {
            if (Preferences == null || Preferences == string.Empty)
            {
                messageService.ShowErrorMessage("Укажите ваши пожелания в улучшении ПК");
                return;
            }

            AddConsultationServiceToShoppingCart(shoppingCartService);
        }

        AddUpgradeServiceToShoppingCart(shoppingCartService);

        messageService.ShowInformationMessage("Апгрейд успешно заказан! Осталось только оплатить");
    }

    private bool CanScan() => !IsScaning;
}