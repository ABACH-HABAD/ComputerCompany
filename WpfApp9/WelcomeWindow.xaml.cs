using Microsoft.Data.Sqlite;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private readonly AuthorizationPage pageAuthorization;
        private readonly RegistrationPage pageRegistration;

        public AuthorizationPage PageAuthorization { get => pageAuthorization; }
        public RegistrationPage PageRegistration { get => pageRegistration; }

        public WelcomeWindow()
        {
            pageAuthorization = new(this);
            pageRegistration = new(this);

            InitializeComponent();

            Random random = new();

            Uri imageUri = new($"/ComputerCompany;component/Resources/Images/WelcomePC{random.Next(1, 5)}.png", UriKind.Relative);

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();

            WelcomeImage.Source = bitmap;

            WelcomePage.Navigate(pageAuthorization);
        }

        public WelcomeWindow(bool AdminRegistersANewAccount = false)
        {
            pageAuthorization = new(this);
            pageRegistration = new(this, AdminRegistersANewAccount);

            InitializeComponent();

            Random random = new();

            Uri imageUri = new($"/ComputerCompany;component/Resources/Images/WelcomePC{random.Next(1, 5)}.png", UriKind.Relative);

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();


            WelcomeImage.Source = bitmap;

            if (AdminRegistersANewAccount) WelcomePage.Navigate(pageRegistration);
            else WelcomePage.Navigate(pageAuthorization);
        }
    }
}
