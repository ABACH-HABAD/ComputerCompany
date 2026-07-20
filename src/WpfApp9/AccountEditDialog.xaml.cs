using ComputerCompanyClasses;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ComputerCompany
{
    /// <summary>
    /// Логика взаимодействия для AccountEditDialog.xaml
    /// </summary>
    public partial class AccountEditDialog : Window
    {
        public string Email => NewEmail.Text;
        public string DisplayName => NewName.Text;
        public string Status => NewStatus.Text;
        public bool ClearImage;


        public AccountEditDialog(string email, string name, string status)
        {
            InitializeComponent();

            NewName.Text = name;
            NewStatus.Text = status;
            NewEmail.Text = email;
        }

        private void DropImage_Click(object sender, RoutedEventArgs e)
        {
            ClearImage = !ClearImage;
            if (ClearImage) DropImage.Content = "Сбросить фото профиля";
            else DropImage.Content = "Оставить фото профиля";
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Account.CheckLogin(Email)) DialogResult = false;
                if (!Account.DisplayAccountTypes.ContainsKey(Status)) DialogResult = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                DialogResult = false;
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
