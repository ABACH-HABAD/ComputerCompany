using ComputerCompany.Application.Client.Abstractions.Servies.Shopping;
using ComputerCompany.Core.Models;

namespace ComputerCompany.Application.Client.Services.Shopping;

public class ShoppingCartService : IShoppingCartService
{
    protected readonly LinkedList<BaseProductModel> _purchases = [];

    public void AddToShoppingCart(BaseProductModel product)
    {
        _purchases.AddLast(product);
    }

    public void RemoveFromShoppingCart<T>(int count = 1) where T : BaseProductModel
    {
        List<T> toRemove = [.. FindByType<T>().Take(count)];

        foreach (T item in toRemove)
        {
            _purchases.Remove(item);
        }
    }

    public void RemoveFromShoppingCart(BaseProductModel product)
    {
        _purchases.Remove(product);
    }

    public void ClearShoppingCart()
    {
        _purchases.Clear();
    }

    public bool InPurchasesList(BaseProductModel product)
    {
        foreach (BaseProductModel model in _purchases)
        {
            if (product.Name == model.Name) return true;
        }
        return false;
    }

    public bool InPurchasesList(string name)
    {
        foreach (BaseProductModel model in _purchases)
        {
            if (model.Name == name) return true;
        }
        return false;
    }

    public IEnumerable<BaseProductModel> GetAll()
    {
        return _purchases;
    }

    public IEnumerable<T> FindByType<T>() where T : BaseProductModel
    {
        return _purchases.OfType<T>();
    }

    public int FindCountByType<T>() where T : BaseProductModel
    {
        return FindByType<T>().Count();
    }

    public double Total()
    {
        double total = 0;
        foreach (BaseProductModel product in _purchases)
        {
            total += product.Price;
        }
        return total;
    }
}