using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Abstractions.Servies.Shoping;

public interface IShoppingCartService
{
    public void AddToShoppingCart(BaseProductModel product);
    public void RemoveFromShoppingCart<T>(int count = 1) where T : BaseProductModel;

    public bool InPurchasesList(BaseProductModel product);
    public bool InPurchasesList(string productName);
    public IEnumerable<T> FindByType<T>() where T : BaseProductModel;
    public int FindCountByType<T>() where T : BaseProductModel;
    public double Total();
}