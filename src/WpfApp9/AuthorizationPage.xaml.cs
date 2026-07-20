using Microsoft.Data.Sqlite;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationUserControl.xaml
    /// </summary>
    public partial class AuthorizationUserControl : UserControl
    {
        private readonly WelcomeWindow welcomeWindow;

        public AuthorizationUserControl(WelcomeWindow welcomeWindow)
        {
            this.welcomeWindow = welcomeWindow;
            InitializeComponent();
        }

        private void Authorization_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text == string.Empty)
            {
                MessageBox.Show("Введите логин");
                return;
            }
            if (Password.Password == string.Empty)
            {
                MessageBox.Show("Введите пароль");
                return;
            }

            try
            {
                Account account = new(Login.Text);
                if (account.Authorization(Login.Text, Password.Password))
                {
                    MessageBox.Show("Вы вошли в аккаунт");
                    MainWindow mainWindow = new(account);
                    mainWindow.Show();
                    welcomeWindow.Close();
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            welcomeWindow.WelcomeUserControl.Navigate(welcomeWindow.UserControlRegistration);
        }
    }
}
