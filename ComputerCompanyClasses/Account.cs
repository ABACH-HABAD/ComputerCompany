using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ComputerCompanyClasses
{
    public class Account
    {
        const string PATTERN_LOGIN = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
        const string PATTERN_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";

        public static readonly Dictionary<string, string> DisplayAccountTypes = new Dictionary<string, string>()
        {
            { "Client", "Клиент"},
            { "Employee", "Сотрудник"},
            { "Cashier", "Кассир"},
            { "Manager", "Менеджер"},
            { "Admin", "Админ"},
        };

        public static readonly Dictionary<string, long> DateBaseAccountTypes = new Dictionary<string, long>()
        {
            { "Client", 1},
            { "Employee",2},
            { "Cashier", 3},
            { "Manager", 4},
            { "Admin", 5},
        };

        protected readonly string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");

        private bool isLogin;
        private string accountType;
        private string displayName;
        private string login;
        private bool hasImage = false;
        private byte[] bitmap;

        public string Login { get => login; set => login = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public bool HasImage { get => hasImage; set => hasImage = value; }
        public byte[] Bitmap { get => bitmap; set => bitmap = value; }
        public string AccountType { get => accountType; set => accountType = value; }
        public string DisplayAccountType { get => DisplayAccountTypes[AccountType]; }
        public bool IsLogin { get => isLogin; }

        public BitmapImage DisplayImage => Image();

        public Account(string login, string displayName = "")
        {
            Login = login;
            DisplayName = displayName;
        }

        public void UpdateData(string Email = "", string Name = "", string Password = "", byte[] Image = null, long AccountType = 0)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand
                {
                    Connection = connection,
                    CommandText = "SELECT id_account FROM Account WHERE email = @email"
                };
                command.Parameters.AddWithValue("@email", login);

                long ID;
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        ID = (long)reader.GetValue(0);
                    }
                    else throw new Exception("Аккаунт не существует");
                }

                if (Email != "") this.Login = Email;
                if (Name != "") this.DisplayName = Name;
                if (Image != null) this.Bitmap = Image;

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {

                    command = new SqliteCommand
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "UPDATE account " +
                        "SET " +
                        $"{(Email != "" ? " email, = @email " : string.Empty)}" +
                        $"{(Name != "" ? " name, = @name" : string.Empty)}" +
                        $"{(Password != "" ? " password = @password, " : string.Empty)}" +
                        $"{(Image != null ? " image = @image, " : string.Empty)}" +
                        $"{(AccountType != 0 ? " account_title = @type, " : string.Empty)}" +
                        " id_account = id_account " + //эта строчка нужна чтобы запятые на вызывали ошибку
                        " WHERE id_account = @id"
                    };
                    command.Parameters.AddWithValue("@id", ID);
                    command.Parameters.AddWithValue("@email", Email);
                    command.Parameters.AddWithValue("@password", Encrypt(Password));
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@image", Image);
                    command.Parameters.AddWithValue("@type", AccountType);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

        public void DropImage()
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = new SqliteCommand
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "UPDATE Account SET image = NULL WHERE email = @email"
                    };
                    command.Parameters.AddWithValue("@email", login);

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

        public BitmapImage Image()
        {
            if (HasImage)
            {
                using (MemoryStream stream = new MemoryStream(Bitmap))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            else return null;
        }

        public bool Authorization(string Login, string Password)
        {
            if (Login == string.Empty || Password == string.Empty) return false;

            if (isLogin) throw new Exception("На данный аккаунт уже выполнен выход");

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT " +
                    "Account.password, " +
                    "Account.name, " +
                    "Account.image, " +
                    "Job_title.name " +
                    "FROM Account " +
                    "INNER JOIN Job_title " +
                    "ON Job_title.id_job_title = Account.account_title " +
                    "WHERE Account.email = @email "
                };
                command.Parameters.AddWithValue("@email", Login);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        string HashedPassword = Encrypt(Password);

                        reader.Read();
                        if (HashedPassword == reader.GetValue(0).ToString())
                        {
                            HasImage = reader.GetValue(2) != DBNull.Value;
                            if (HasImage)
                            {
                                Bitmap = (byte[])reader.GetValue(2);
                            }

                            if (reader.GetValue(3) != DBNull.Value) DisplayName = (string)reader.GetValue(1);
                            else DisplayName = Login;

                            if (reader.GetValue(3) != DBNull.Value) AccountType = (string)reader.GetValue(3);
                            else AccountType = "Client";

                            isLogin = true;

                            return true;
                        }
                        else
                        {
                            throw new Exception("Неверный логин или пароль");
                        }
                    }
                    else
                    {
                        throw new Exception("Аккаунт не найден");
                    }
                }
            }
        }

        public bool Registration(string Login, string Password, string Password2)
        {
            if (!CheckPasswordAndLogin(Login, Password, Password2)) return false;

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = $"SELECT email FROM account WHERE email = @email"
                };
                command.Parameters.AddWithValue("@email", Login);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        throw new Exception("Такой аккаунт уже зарегистрирован");
                    }
                    else
                    {
                        string HashedPassword = Encrypt(Password);

                        string Name = string.Empty;
                        foreach (char c in Login)
                            if (c == '@') break;
                            else Name += c;

                        this.displayName = Name;

                        using (SqliteTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                command = new SqliteCommand()
                                {
                                    Connection = connection,
                                    CommandText = "INSERT INTO account (email, password, name, account_title) VALUES (@email, @password, @name, 1)",
                                    Transaction = transaction
                                };
                                command.Parameters.AddWithValue("@email", Login);
                                command.Parameters.AddWithValue("@password", HashedPassword);
                                command.Parameters.AddWithValue("@name", Name);

                                command.ExecuteNonQuery();
                                transaction.Commit();

                                AccountType = "Client";
                                isLogin = true;

                                return true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw new Exception($"Ошибка при регистрации: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        public void LogOut()
        {
            isLogin = false;
        }

        public bool Delete(string ConfirmationPassword)
        {

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT password FROM Account WHERE email = @email"
                };
                command.Parameters.AddWithValue("@email", Login);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        if ((string)reader.GetValue(0) != Encrypt(ConfirmationPassword))
                        {
                            throw new Exception("Подтвердите пароль для входа в аккаунт");
                        }
                    }
                    else throw new Exception("Аккаунт не существует");
                }

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    command = new SqliteCommand()
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "DELETE FROM Account WHERE email = @email"
                    };
                    command.Parameters.AddWithValue("@email", Login);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                    LogOut();

                    return true;
                }
            }
        }

        public bool Delete(Account adminAccount)
        {
            if (adminAccount.AccountType != "Admin") throw new Exception("Для этого требуются права администратора");

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = new SqliteCommand()
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "DELETE FROM Account WHERE email = @email"
                    };
                    command.Parameters.AddWithValue("@email", Login);

                    command.ExecuteNonQuery();

                    transaction.Commit();

                    LogOut();

                    return true;
                }
            }
        }

        private string Encrypt(string password)
        {
            string HashedPassword;
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                HashedPassword = Convert.ToBase64String(hashBytes);
            }
            return HashedPassword;
        }

        public static bool CheckLogin(string Login)
        {
            if (Login == string.Empty)
            {
                throw new Exception("Введите логин");
            }
            if (!Regex.IsMatch(Login, PATTERN_LOGIN, RegexOptions.IgnoreCase))
            {
                throw new Exception("Некорректный логин.\n" +
                    " Логин должен содержать вашу почту");
            }

            return true;
        }

        public static bool CheckPassword(string Password)
        {
            if (Password == string.Empty) throw new Exception("Введите пароль");
            if (!Regex.IsMatch(Password, PATTERN_PASSWORD, RegexOptions.IgnoreCase)) throw new Exception("Некорректный пароль.\n" + " Пароль должен содержать: Строчную и заглавную букву, цифру, спец-символ и быть длинной не менее 8 и не более 15 символов");

            return true;
        }

        public static bool CheckPassword(string Password1, string Password2)
        {
            CheckPassword(Password1);
            try
            {
                CheckPassword(Password2);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message == "Введите пароль" ? "Повторите пароль" : ex.Message);
            }
            if (Password2 != Password1) throw new Exception("Пароли не совпадают");

            return true;
        }

        public static bool CheckPasswordAndLogin(string Login, string Password)
        {
            CheckLogin(Login);
            CheckPassword(Password);

            return true;
        }

        public static bool CheckPasswordAndLogin(string Login, string Password1, string Password2)
        {
            CheckLogin(Login);
            CheckPassword(Password1, Password2);

            return true;
        }

        public static Account[] GetAllAccounts(Account sender)
        {
            if (sender.AccountType != "Admin") throw new Exception("Недостаточно полномочий для доступа к базе данных");

            List<Account> accounts = new List<Account>();
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT " +
                    "Account.email, " +
                    "Account.name, " +
                    "Account.image, " +
                    "Job_title.name AS title " +
                    "FROM Account " +
                    "INNER JOIN Job_title " +
                    "ON Job_title.id_job_title = Account.account_title "
                };

                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            accounts.Add(new Account((string)reader["email"])
                            {
                                DisplayName = (string)reader["name"],
                                HasImage = reader["image"] != DBNull.Value,
                                Bitmap = (reader["image"] != DBNull.Value ? (byte[])reader["image"] : null),
                                AccountType = (string)reader["title"]
                            });
                        }
                        catch { }
                    }
                }
            }

            return accounts.ToArray();
        }
    }
}
