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
        private readonly AuthorizationUserControl UserControlAuthorization;
        private readonly RegistrationUserControl UserControlRegistration;

        public AuthorizationUserControl UserControlAuthorization { get => UserControlAuthorization; }
        public RegistrationUserControl UserControlRegistration { get => UserControlRegistration; }

        public WelcomeWindow()
        {
            UserControlAuthorization = new(this);
            UserControlRegistration = new(this);

            InitializeComponent();

            Random random = new();

            Uri imageUri = new($"/ComputerCompany;component/Resources/Images/WelcomePC{random.Next(1, 5)}.png", UriKind.Relative);

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();

            WelcomeImage.Source = bitmap;

            WelcomeUserControl.Navigate(UserControlAuthorization);
        }

        public WelcomeWindow(bool AdminRegistersANewAccount = false)
        {
            UserControlAuthorization = new(this);
            UserControlRegistration = new(this, AdminRegistersANewAccount);

            InitializeComponent();

            Random random = new();

            Uri imageUri = new($"/ComputerCompany;component/Resources/Images/WelcomePC{random.Next(1, 5)}.png", UriKind.Relative);

            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = imageUri;
            bitmap.EndInit();


            WelcomeImage.Source = bitmap;

            if (AdminRegistersANewAccount) WelcomeUserControl.Navigate(UserControlRegistration);
            else WelcomeUserControl.Navigate(UserControlAuthorization);
        }
    }
}
