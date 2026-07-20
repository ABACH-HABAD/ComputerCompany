using System.Diagnostics;
using System.Windows.Input;
using ComputerCompany.Application.Client.Services.Shoping;
using ComputerCompany.Application.Client.ViewModels.Commands;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class UpgradeViewModel : BaseUserControlViewModel
{
    private bool _consultationMode;
    private bool _iKnowWhatIWantMode;

    private string _cpu;
    private string _gpu;
    private string _motherboard;
    private string _memory;

    private string _preferences;

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

    public string Memoty
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

    public UpgradeViewModel()
    {
        AutoScanCommand = new RelayCommand(AutoScan, CanScan);
        OrderUpgradeCommand = new RelayCommand(OrderUpgrade);
    }

    private void AutoScan()
    {
        IsScaning = true;
        Process scanner = Process.Start("dxdiag");
        scanner.Exited += (s, e) => IsScaning = false;
    }

    private void OrderUpgrade()
    {
        /*
            if (!ShoppingCartService.InPurchasesList("Улучшение ПК"))
            {
                Product upgrade = new("Улучшение ПК", 7500);
                upgrade.AddToShoppingCart();
            }

            if (Consultation.IsChecked == true)
            {
                if (!ShoppingCartService.InPurchasesList("Консультация"))
                {
                    Product consultation = new("Консультация", 3000);
                    consultation.AddToShoppingCart();
                }
                else MessageBox.Show("Вы уже заказали консультацию");
            }

            MessageBox.Show("Апгрейд успешно заказан! Осталось только оплатить");*/
    }

    private bool CanScan() => !IsScaning;
}