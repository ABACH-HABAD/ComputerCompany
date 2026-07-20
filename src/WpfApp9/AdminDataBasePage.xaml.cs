using ComputerCompany;
using ComputerCompanyClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для AdminDataBaseUserControl.xaml
    /// </summary>
    public partial class AdminDataBaseUserControl : UserControl
    {
        private readonly ObservableCollection<Account> accounts = [];

        private readonly Account adminAccount;

        public ObservableCollection<Account> Accounts { get => accounts; }

        public AdminDataBaseUserControl(Account adminAccount)
        {
            InitializeComponent();

            if (adminAccount.AccountType != "Admin") throw new Exception("Невозможно открыть окно админа: недостаточно полномочий");
            this.adminAccount = adminAccount;

            accounts = new ObservableCollection<Account>(Account.GetAllAccounts(adminAccount));
            DataContext = this;
        }

        private void UpdateAccountList()
        {
            accounts.Clear();
            foreach (var account in Account.GetAllAccounts(adminAccount))
            {
                accounts.Add(account);
            }
            DataContext = this;
        }

        private void PasswordReset(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is Account account)
                {
                    PasswordResetDialog dialog = new();
                    if (dialog.ShowDialog() == true)
                    {
                        account.UpdateData(Password: dialog.Password);
                        UpdateAccountList();
                    }
                }
                else
                {
                    if (button != null)
                    {
                        button.IsEnabled = false;
                        button.ToolTip = "Нельзя сбросить пароль данному аккаунту";
                        MessageBox.Show((string)button.ToolTip);
                    }
                }
            }

        }

        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is Account account)
                {
                    AccountEditDialog dialog = new(account.Login, account.DisplayName, account.AccountType);
                    if (dialog.ShowDialog() == true)
                    {
                        if (dialog.ClearImage) account.DropImage();
                        account.UpdateData(Email: dialog.Email, Name: dialog.DisplayName, AccountType: Account.DateBaseAccountTypes[dialog.Status]);
                        UpdateAccountList();
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is Account account)
                {
                    account.Delete(adminAccount);
                    UpdateAccountList();
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            WelcomeWindow welcomeWindow = new(true);
            if (welcomeWindow.ShowDialog() == true)
            {
                UpdateAccountList();
            }
        }
    }
}
