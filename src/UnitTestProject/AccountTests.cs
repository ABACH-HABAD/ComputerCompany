using ComputerCompanyClasses;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class AccountTests
    {
        //Подключение к БД для тестов
        public static SqliteConnection CreateTestConnection()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db");
            string connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared";
            return new SqliteConnection(connectionString);
        }

        //Обязательная проверка, что аккаунт не существует
        private bool CheckAccount(string Email, string Password)
        {
            try
            {
                Account account = new Account(Email);
                if (account.Authorization(Email, Password)) 
                    account.Delete(Password);
                return true;
            }
            catch
            { 
                return false; 
            }
        }

        [TestMethod]
        public void RegistrationAccount()
        {
            // Arrange
            string Email = "UnitTestAccount@mail.com";
            string Password = "Aaa12345%";
            CheckAccount(Email, Password);

            // Act
            Account account = new Account(Email);
            bool RegistrationResult = account.Registration(Email, Password, Password);
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(true,RegistrationResult);
        }

        [TestMethod]
        public void RegistrationAccountWithoutEmail()
        {
            // Arrange
            string Email = string.Empty;
            string Password = "Aaa12345%";
            string ExpectedMessage = "Введите логин";
            CheckAccount(Email, Password);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                bool RegistrationResult = account.Registration(Email, Password, Password);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }

        [TestMethod]
        public void RegistrationAccountWithoutPassword()
        {
            // Arrange
            string Email = "UnitTestAccount@mail.com";
            string Password = string.Empty;
            string ExpectedMessage = "Введите пароль";
            CheckAccount(Email, Password);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                bool RegistrationResult = account.Registration(Email, Password, Password);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }

        [TestMethod]
        public void RegistrationAccountWithWrongEmail()
        {
            // Arrange
            string Email = "UnitTestAccount";
            string Password = "Aaa12345%";
            string ExpectedMessage = "Некорректный логин.\n" + " Логин должен содержать вашу почту";
            CheckAccount(Email, Password);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                bool RegistrationResult = account.Registration(Email, Password, Password);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }

        [TestMethod]
        public void RegistrationAccountWithWrongPassword()
        {
            // Arrange
            string Email = "UnitTestAccount@mail.com";
            string Password = "123";
            string ExpectedMessage = "Некорректный пароль.\n" + " Пароль должен содержать: Строчную и заглавную букву, цифру, спец-символ и быть длинной не менее 8 и не более 15 символов";
            CheckAccount(Email, Password);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                bool RegistrationResult = account.Registration(Email, Password, Password);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }

        [TestMethod]
        public void RegistrationAccountWithDifferentPasswords()
        {
            // Arrange
            string Email = "UnitTestAccount@mail.com";
            string Password1 = "Aaa12345%";
            string Password2 = "Aaa54321%";
            string ExpectedMessage = "Пароли не совпадают";
            CheckAccount(Email, Password1);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                bool RegistrationResult = account.Registration(Email, Password1, Password2);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password1); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }

        [TestMethod]
        public void RegistrationTwoAccountWithOneEmail()
        {
            // Arrange
            string Email = "UnitTestAccount@mail.com";
            string Password = "Aaa12345%";
            string ExpectedMessage = "Такой аккаунт уже зарегистрирован";
            CheckAccount(Email, Password);

            // Act
            string Message = string.Empty;
            Account account = new Account(Email);
            try
            {
                account.Registration(Email, Password, Password);
                bool RegistrationResult = account.Registration(Email, Password, Password);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            try { account.Delete(Password); } catch { }

            // Assert
            Assert.AreEqual(ExpectedMessage, Message);
        }
    }
}
