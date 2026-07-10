using ComputerCompanyClasses;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class AssemblyTests
    {
        //Подключение к БД для тестов
        public static SqliteConnection CreateTestConnection()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db");
            string connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared";
            return new SqliteConnection(connectionString);
        }

        private void CreateComponents()
        {
            ShoppingCart.Purchases.Clear();

            CPU cpu = new CPU() { Model = "Intel Core I3", Socket = "LGA 1700", Price = 1000, Count = 5 };
            GPU gpu = new GPU() { Model = "Palit GameRock", ModelCore = "RTX 4080", Price = 1000, Count = 5 };
            Memory memory = new Memory() { Model = "Radeon", Size = 1024, Type = "DDR5", Price = 1000, Count = 5 };
            Storage storage = new Storage() { Model = "Samsung", Form_factor = "3.5\"", Type = "SSD", Size = 1000, Price = 1000, Count = 5 };
            Power_unit power = new Power_unit() { Model = "KCAS", Power = 1000, Certification="80+", Form_factor = "ATX", Price = 1000, Count = 5 };
            Motherboard motherboard = new Motherboard() { Model = "Gigabyte B650", Chipset="B650" ,Socket = "LGA 1700", Price = 1000, Count = 5 };
            Frame frame = new Frame() { Model = "LianLi", Form_factor = "ATX", Price = 1000, Count = 5 };

            if (!cpu.FindInDataBase()) cpu.AddToDataBase();
            if (!gpu.FindInDataBase()) gpu.AddToDataBase();
            if (!memory.FindInDataBase()) memory.AddToDataBase();
            if (!storage.FindInDataBase()) storage.AddToDataBase();
            if (!power.FindInDataBase()) power.AddToDataBase();
            if (!motherboard.FindInDataBase()) motherboard.AddToDataBase();
            if (!frame.FindInDataBase()) frame.AddToDataBase();

            cpu.AddToShoppingCart();
            gpu.AddToShoppingCart();
            memory.AddToShoppingCart();
            memory.AddToShoppingCart(); //we need 2 planks of RAM
            storage.AddToShoppingCart();
            power.AddToShoppingCart();
            motherboard.AddToShoppingCart();
            frame.AddToShoppingCart();
        }

        [TestMethod]
        public void NormalAssembly()
        {
            // Arrange
            CreateComponents();
            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            bool expectation = true;

            // Act
            bool result;
            try
            {
                assembly.SendInDataBase();
                result = true;
            }
            catch
            {
                result = false;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithoutMemory()
        {
            // Arrange
            ShoppingCart.Purchases.Clear();

            CPU cpu = new CPU() { Model = "Intel Core I3", Socket = "LGA 1700", Price = 1000, Count = 5 };
            GPU gpu = new GPU() { Model = "Palit GameRock", ModelCore = "RTX 4080", Price = 1000, Count = 5 };
            Storage storage = new Storage() { Model = "Samsung", Type = "SSD", Size = 1000, Price = 1000, Count = 5 };
            Power_unit power = new Power_unit() { Model = "KCAS", Power = 1000, Form_factor = "ATX", Price = 1000, Count = 5 };
            Motherboard motherboard = new Motherboard() { Model = "Gigabyte B650", Socket = "LGA 1700", Price = 1000, Count = 5 };
            Frame frame = new Frame() { Model = "LianLi", Form_factor = "ATX", Price = 1000, Count = 5 };

            cpu.AddToShoppingCart();
            gpu.AddToShoppingCart();
            storage.AddToShoppingCart();
            power.AddToShoppingCart();
            motherboard.AddToShoppingCart();
            frame.AddToShoppingCart();

            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            string expectation = "Не выбрана оперативная память";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckAssemblyCompleteness();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithoutCPU()
        {
            // Arrange
            ShoppingCart.Purchases.Clear();

            GPU gpu = new GPU() { Model = "Palit GameRock", ModelCore = "RTX 4080", Price = 1000, Count = 5 };
            Memory memory = new Memory() { Model = "Radeon", Size = 1024, Type = "DDR5", Price = 1000, Count = 5 };
            Storage storage = new Storage() { Model = "Samsung", Type = "SSD", Size = 1000, Price = 1000, Count = 5 };
            Power_unit power = new Power_unit() { Model = "KCAS", Power = 1000, Form_factor = "ATX", Price = 1000, Count = 5 };
            Motherboard motherboard = new Motherboard() { Model = "Gigabyte B650", Socket = "LGA 1700", Price = 1000, Count = 5 };
            Frame frame = new Frame() { Model = "LianLi", Form_factor = "ATX", Price = 1000, Count = 5 };

            gpu.AddToShoppingCart();
            memory.AddToShoppingCart();
            memory.AddToShoppingCart();
            storage.AddToShoppingCart();
            power.AddToShoppingCart();
            motherboard.AddToShoppingCart();
            frame.AddToShoppingCart();

            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            string expectation = "Не выбран процессор";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckAssemblyCompleteness();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithDifferentSockets()
        {
            // Arrange
            CreateComponents();
            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            assembly.UsedMotherboard.Socket = "AM5";
            string expectation = "Сокет на материнской плате не соответствует сокету процессора";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithWrongPowerUnit()
        {
            // Arrange
            CreateComponents();
            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            assembly.UsedPowerUnit.Form_factor = "mini ATX";
            string expectation = "Блок питания не подходит к корпусу";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithManyRAM()
        {
            // Arrange
            CreateComponents();

            Memory memory = new Memory() { Model = "Radeon", Size = 1024, Type = "DDR5", Price = 1000, Count = 5 };
            for (int i = 0; i < 10; i++)
            {
                memory.AddToShoppingCart();
            }

            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            string expectation = "Слишком много оперативной памяти";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }

        [TestMethod]
        public void AssemblyWithOddRAM()
        {
            // Arrange
            CreateComponents();

            Memory memory = new Memory() { Model = "Radeon", Size = 1024, Type = "DDR5", Price = 1000, Count = 5 };
            memory.AddToShoppingCart();

            Assembly assembly = new Assembly();
            assembly.FindAssemblyInShoppingCart();
            string expectation = "Количество планок оперативной памяти должно быть чётным";

            // Act
            string result = string.Empty;
            try
            {
                assembly.CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            // Assert
            Assert.AreEqual(expectation, result);
        }
    }
}
