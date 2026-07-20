using System;
using System.Collections.Generic;
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

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для ClientBuildUserControl.xaml
    /// </summary>
    public partial class ClientBuildUserControl : UserControl
    {
        private readonly Assembly assembly = new();

        public ClientBuildUserControl()
        {
            InitializeComponent();
        }

        private void ConsultationRB_Checked(object sender, RoutedEventArgs e)
        {
            if (ConsultationRB.IsChecked == true)
            {
                Consultation.Visibility = Visibility.Visible;
                INowWhatIWant.Visibility = Visibility.Hidden;
            }
        }

        private void IKnowWhatIWantRB_Checked(object sender, RoutedEventArgs e)
        {
            if (IKnowWhatIWantRB.IsChecked == true)
            {
                Consultation.Visibility = Visibility.Hidden;
                INowWhatIWant.Visibility = Visibility.Visible;
            }
        }

        private void OrderAssembly_Click(object sender, RoutedEventArgs e)
        {
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

            if (!ShoppingCart.InPurchasesList("Сборка ПК"))
            {
                Product upgrade = new("Сборка ПК", 7500);
                upgrade.AddToShoppingCart();
            }

            if (ConsultationRB.IsChecked == true)
            {
                if (!ShoppingCart.InPurchasesList("Консультация"))
                {
                    Product consultation = new("Консультация", 3000);
                    consultation.AddToShoppingCart();
                }
                else MessageBox.Show("Вы уже заказали консультацию");
            }

            MessageBox.Show("Сборка ПК успешно заказана! Осталось только оплатить");
        }
    }
}
