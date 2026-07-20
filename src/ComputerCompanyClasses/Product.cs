using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerCompanyClasses
{
    public class Product
    {

        private string description;
        private long price;

        public string Description => GenerateDescription();

        public long Price { get => price; set => price = value; }

        public string Name => ToString();

        public Product() { }

        public Product(string description)
        {
            this.description = description;
        }

        public Product(string description, long price)
        {
            this.description = description;
            this.price = price;
        }

        public void AddToShoppingCart()
        {
            ShoppingCart.Purchases.Add(this);
        }

        public void AddToShoppingCart(string description)
        {
            this.description = description;
            ShoppingCart.Purchases.Add(this);
        }

        public string GenerateDescription()
        {
            if (description == null)
            {
                description = Name;
            }
            else
            {
                if (description == string.Empty)
                {
                    description = Name;
                }
            }
            return description;
        }

        public void RemoveFromShoppingCard()
        {
            if (ShoppingCart.InPurchasesList(this.Description))
            {
                try
                {
                    ShoppingCart.Purchases.Remove(this);
                }
                catch
                {
                    foreach (Product product in ShoppingCart.Purchases)
                    {
                        if (product.Description == description)
                        {
                            ShoppingCart.Purchases.Remove(product);
                            break;
                        }
                    }
                }
            }
        }
    }
}
