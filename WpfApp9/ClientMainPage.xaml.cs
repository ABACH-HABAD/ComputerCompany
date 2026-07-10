using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ClientMainPage.xaml
    /// </summary>
    public partial class ClientMainPage : Page
    {
        private readonly Navigate toBuildingPage;
        private readonly Navigate toUpgradePage;
        //private readonly Navigate toRepairPage; //еще не готово
        private readonly Navigate toComponentPage;

        public ClientMainPage(Navigate toBuildingPage, Navigate toUpgradePage, Navigate toComponentPage)
        {
            this.toBuildingPage = toBuildingPage;
            this.toUpgradePage = toUpgradePage;
            this.toComponentPage = toComponentPage;
            InitializeComponent();
        }

        public delegate void Navigate(object sender, RoutedEventArgs e);

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? search = new(new string(Search.Text).ToLower());
            foreach (Button button in AllServices.Children)
            {
                string? content = new(new string(button.Content.ToString()).ToLower());

                if (search.Length == 0)
                {
                    button.Visibility = Visibility.Visible;
                    continue;
                }

                if (Regex.IsMatch(content, search)) button.Visibility = Visibility.Visible;
                else button.Visibility = Visibility.Hidden;
            }
        }

        private void Building_Click(object sender, RoutedEventArgs e)
        {
            toBuildingPage(sender, e);
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            toUpgradePage(sender, e);
        }

        private void Repair_Click(object sender, RoutedEventArgs e)
        {
            //toRepairPage(sender, e);
            MessageBox.Show("Услуга \"Починка ПК\" на данный момент недоступна");
        }

        private void Component_Click(object sender, RoutedEventArgs e)
        {
            toComponentPage(sender, e);
        }
    }
}
