using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly WelcomeWindow welcomeWindow;
        private readonly bool adminRegistersANewAccount;

        public RegistrationPage(WelcomeWindow welcomeWindow, bool adminRegistersANewAccount = false)
        {
            this.welcomeWindow = welcomeWindow;
            this.adminRegistersANewAccount = adminRegistersANewAccount;
            InitializeComponent();

            if (adminRegistersANewAccount) AutorizationButton.Visibility = Visibility.Hidden;
        }

        public void Registration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Account account = new(Login.Text);
                if (account.Registration(Login.Text, Password.Password, Password2.Password))
                {
                    if (adminRegistersANewAccount) welcomeWindow.DialogResult = true;
                    else
                    {
                        MessageBox.Show("Аккаунт успешно зарегистрирован!");
                        MainWindow mainWindow = new(account);
                        mainWindow.Show();
                    }

                    welcomeWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Authorization_Click(object sender, RoutedEventArgs e)
        {
            welcomeWindow.WelcomePage.Navigate(welcomeWindow.PageAuthorization);
        }
    }
}
