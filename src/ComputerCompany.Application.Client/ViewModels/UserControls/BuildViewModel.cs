using System.Windows.Input;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.Services.Shoping;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Abstractions.Services;

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


    private async Task OrderAssemblyAsync()
    {
        /*
            try
            {
                assembly.FindAssemblyInShoppingCart();
                assembly.CheckAssemblyCompleteness();
                assembly.CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            assembly.SendInDataBase();

            if (!ShoppingCartService.InPurchasesList("Сборка ПК"))
            {
                Product upgrade = new("Сборка ПК", 7500);
                upgrade.AddToShoppingCart();
            }

            if (ConsultationRB.IsChecked == true)
            {
                if (!ShoppingCartService.InPurchasesList("Консультация"))
                {
                    Product consultation = new("Консультация", 3000);
                    consultation.AddToShoppingCart();
                }
                else MessageBox.Show("Вы уже заказали консультацию");
            }

            MessageBox.Show("Сборка ПК успешно заказана! Осталось только оплатить");*/
    }
}