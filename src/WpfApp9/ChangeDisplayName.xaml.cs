using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ChangeDisplayName.xaml
    /// </summary>
    public partial class ChangeDisplayName : Window
    {
        private readonly Profile profile;

        public ChangeDisplayName(Profile profile)
        {
            InitializeComponent();

            this.profile = profile;
            if (profile.CurrentAccount.AccountType == "Client")
            {
                FirstNameLable.Content = "Отображаемое имя:";
            }
            else
            {
                FirstNameLable.Content = "Имя:";
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            profile.CurrentAccount.DisplayName = FirstName.Text;

            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");
            using SqliteConnection connection = new($"Data Source={dbPath}");
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                SqliteCommand command = new()
                {
                    Connection = connection,
                    CommandText = $"UPDATE Account SET name = @name WHERE email = @email;",
                    Transaction = transaction
                };
                command.Parameters.AddWithValue("@email", profile.CurrentAccount.Login);
                command.Parameters.AddWithValue("@name", FirstName.Text);

                command.ExecuteNonQuery();
                transaction.Commit();
                MessageBox.Show("Имя изменено успешно!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Ошибка при смене имени: {ex.Message}");
            }

            profile.Main.UpdateAccountInfo();
            profile.AccountName.Text = profile.CurrentAccount.DisplayName;
            Close();
        }

        private void Declare_Click(object sender, RoutedEventArgs e)
        {
            profile.Main.UpdateAccountInfo();
            profile.AccountName.Text = profile.CurrentAccount.DisplayName;
            Close();
        }
    }
}
