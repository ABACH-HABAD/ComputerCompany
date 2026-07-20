using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для ClientUpgradeUserControl.xaml
    /// </summary>
    public partial class ClientUpgradeUserControl : UserControl
    {
        public ClientUpgradeUserControl()
        {
            InitializeComponent();
        }

        private void AutoScan_click(object sender, RoutedEventArgs e)
        {
            Process.Start("dxdiag");
        }

        private void OrderUpgrade_Click(object sender, RoutedEventArgs e)
        {
            if (!ShoppingCart.InPurchasesList("Улучшение ПК"))
            {
                Product upgrade = new("Улучшение ПК", 7500);
                upgrade.AddToShoppingCart();
            }
            
            if (Consultation.IsChecked == true)
            {
                if (!ShoppingCart.InPurchasesList("Консультация"))
                {
                    Product consultation = new("Консультация", 3000);
                    consultation.AddToShoppingCart();
                }
                else MessageBox.Show("Вы уже заказали консультацию");
            }

            MessageBox.Show("Апгрейд успешно заказан! Осталось только оплатить");
        }
    }
}
