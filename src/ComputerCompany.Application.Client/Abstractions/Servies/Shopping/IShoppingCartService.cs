using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Shopping;

public interface IShoppingCartService
{
    internal const string PcAssemblyServiceName = "Сборка ПК";
    internal const string PcAssemblyServiceDescription = "Сборк готового персонального компьютера из выбранных комплектующих";
    internal const double PcAssemblyServicePrice = 7500;

    internal const string PcUpgradeServiceName = "Улучшение ПК";
    internal const string PcUpgradeServiceDescription = "Установка новых комплектующих в старый ПК";
    internal const double PcUpgradeServicePrice = 7500;

    internal const string PcConsultationServiceName = "Консультация";
    internal const string PcConsultationServiceDescription = "Консультация по подборку готовых комплектующих для сборки ПК";
    internal const double PcConsultationServicePrice = 3000;

    public void AddToShoppingCart(BaseProductModel product);
    public void RemoveFromShoppingCart<T>(int count = 1) where T : BaseProductModel;
    public void RemoveFromShoppingCart(BaseProductModel product);
    public void ClearShoppingCart();

    public bool InPurchasesList(BaseProductModel product);
    public bool InPurchasesList(string productName);
    public IEnumerable<BaseProductModel> GetAll(); 
    public IEnumerable<T> FindByType<T>() where T : BaseProductModel;
    public int FindCountByType<T>() where T : BaseProductModel;
    public double Total();
}