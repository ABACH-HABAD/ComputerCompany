using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using ComputerCompany.Core.Models;
using ComputerCompany.Application.Abstractions.Services;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.ViewModels.Dialogs;
using ComputerCompany.Application.Results;

namespace ComputerCompany.Application.Client.ViewModels.UserControls;

public class ShoppingCartViewModel : BaseUserControlViewModel
{
    private readonly IScopeFactory _scopeFactory;

    private bool _isPaying;
    private double _total;

    public ObservableCollection<BaseProductModel> Products { get; } = [];

    public bool IsPaying
    {
        get => _isPaying;
        set => ChangeProperty(ref _isPaying, value);
    }

    public double Total
    {
        get => _total;
        set => ChangeProperty(ref _total, value);
    }

    public ICommand LoadProdcutsFromShoppingCartCommand { get; }
    public ICommand ClearBasketCommand { get; }
    public ICommand DeleteProductFromShoppingCartCommand { get; }
    public ICommand CardPayCommand { get; }
    public ICommand QrPayCommand { get; }

    public ShoppingCartViewModel(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        LoadProdcutsFromShoppingCartCommand = new RelayCommand(LoadProdcutsFromShoppingCart);
        ClearBasketCommand = new RelayCommand(ClearShoppingCart);
        DeleteProductFromShoppingCartCommand = new RelayCommand<BaseProductModel>(DeleteProductFromShoppingCart);
        CardPayCommand = new AsyncRelayCommand(CardPayAsync, CanPay);
        QrPayCommand = new AsyncRelayCommand(QrPayAsync, CanPay);

        LoadProdcutsFromShoppingCartCommand.Execute(null);
    }

    private void LoadProdcutsFromShoppingCart()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IShoppingCartService shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

        Products.Clear();
        foreach (BaseProductModel product in shoppingCartService.GetAll())
        {
            Products.Add(product);
        }
        Total = shoppingCartService.Total();
    }

    private void ClearShoppingCart()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IShoppingCartService shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

        shoppingCartService.ClearShoppingCart();

        LoadProdcutsFromShoppingCartCommand.Execute(null);
    }

    private void DeleteProductFromShoppingCart(BaseProductModel product)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        IShoppingCartService shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();

        shoppingCartService.RemoveFromShoppingCart(product);

        LoadProdcutsFromShoppingCartCommand.Execute(null);
    }

    private async Task CardPayAsync()
    {
        _isPaying = true;
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();
            IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

            Result dialogResult = await dialogService.ShowInformationDialogAsync<CardPayViewModel>("Оплата картой");
            if (dialogResult.IsSuccess)
            {
                messageService.ShowInformationMessage("Производится оплата, пожалуйста подождите");
                await Task.Delay(5000);
                messageService.ShowInformationMessage("Оплата прошла успешно! Спасибо за покупку!");
                ClearBasketCommand.Execute(null);
            }
            else
            {
                if (dialogResult.Message != null && dialogResult.Message != string.Empty) messageService.ShowErrorMessage(dialogResult.Message);
            }
        }
        finally
        {
            _isPaying = false;
        }
    }

    private async Task QrPayAsync()
    {
        _isPaying = true;
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            IDialogService dialogService = scope.ServiceProvider.GetRequiredService<IDialogService>();

            await dialogService.ShowInformationDialogAsync<QrViewModel>("Оплата QR кодом");
        }
        finally
        {
            _isPaying = false;
        }
    }

    private bool CanPay() => !_isPaying;
}