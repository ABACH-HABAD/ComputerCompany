using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerCompanyClasses
{
    public class ShoppingCart
    {
        public static List<Product> Purchases = new List<Product>();

        public static bool InPurchasesList(string purchaseDescription)
        {
            if (Purchases.Count > 0)
            {
                foreach (Product product in Purchases)
                {
                    if (product.Description == purchaseDescription) return true;
                }
                return false;
            }
            else return false;
        }

        public static Product FindByType(Type T)
        {
            if (!T.IsSubclassOf(typeof(Product))) throw new Exception("Должен быть только тип, наследуемый от Product");

            foreach (Product product in Purchases)
            {
                if (product.GetType() == T) return product;
            }
            return null;
        }

        public static int FindCountByType(Type T)
        {
            if (!T.IsSubclassOf(typeof(Product))) throw new Exception("Должен быть только тип, наследуемый от Product");

            int count = 0;
            foreach (Product product in Purchases)
            {
                if (product.GetType() == T) count++;
            }
            return count;
        }

        public static long Total()
        {
            long sum = 0;
            foreach (Product purchase in Purchases)
            {
                sum += purchase.Price;
            }
            return sum;
        }

        public static long Total(List<Product> products)
        {
            long sum = 0;
            foreach (Product purchase in products)
            {
                sum += purchase.Price;
            }
            return sum;
        }
    }
}
