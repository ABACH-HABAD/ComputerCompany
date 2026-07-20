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
    /// Логика взаимодействия для ClientMainUserControl.xaml
    /// </summary>
    public partial class ClientMainUserControl : UserControl
    {
        private readonly Navigate toBuildingUserControl;
        private readonly Navigate toUpgradeUserControl;
        //private readonly Navigate toRepairUserControl; //еще не готово
        private readonly Navigate toComponentUserControl;

        public ClientMainUserControl(Navigate toBuildingUserControl, Navigate toUpgradeUserControl, Navigate toComponentUserControl)
        {
            this.toBuildingUserControl = toBuildingUserControl;
            this.toUpgradeUserControl = toUpgradeUserControl;
            this.toComponentUserControl = toComponentUserControl;
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
            toBuildingUserControl(sender, e);
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            toUpgradeUserControl(sender, e);
        }

        private void Repair_Click(object sender, RoutedEventArgs e)
        {
            //toRepairUserControl(sender, e);
            MessageBox.Show("Услуга \"Починка ПК\" на данный момент недоступна");
        }

        private void Component_Click(object sender, RoutedEventArgs e)
        {
            toComponentUserControl(sender, e);
        }
    }
}
