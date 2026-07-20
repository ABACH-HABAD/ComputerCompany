using ComputerCompany;
using ComputerCompanyClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для ClientShoppingCartUserControl.xaml
    /// </summary>
    public partial class ClientShoppingCartUserControl : UserControl
    {
        private readonly ObservableCollection<Product> Purchases = [];

        public ClientShoppingCartUserControl()
        {
            InitializeComponent();

            UpdatePurchases();
            UpdateTotal();
        }

        public void UpdatePurchases()
        {
            Purchases.Clear();
            foreach (Product product in ShoppingCart.Purchases)
            {
                Purchases.Add(product);
            }
            PurchaseList.ItemsSource = Purchases;
        }

        private void UpdateTotal()
        {
            Total.Text = $"{ShoppingCart.Total()}";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is Product product)
                {
                    Purchases.Remove(product);
                    ShoppingCart.Purchases.Remove(product);
                }
            }
            UpdateTotal();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePurchases();
            UpdateTotal();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCart.Purchases.Clear();
            UpdateTotal();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdatePurchases();
            UpdateTotal();
        }

        private void CardPayButton_Click(object sender, RoutedEventArgs e)
        {
            CardPay pay = new();
            pay.ShowDialog();
        }

        private void QRCodePayButton_Click(object sender, RoutedEventArgs e)
        {
           QRPay pay = new();
            pay.ShowDialog();
        }
    }
}
