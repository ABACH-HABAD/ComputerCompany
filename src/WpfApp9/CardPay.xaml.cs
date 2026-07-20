using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerCompany
{
    /// <summary>
    /// Логика взаимодействия для CardPay.xaml
    /// </summary>
    public partial class CardPay : Window
    {
        public CardPay()
        {
            InitializeComponent();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
