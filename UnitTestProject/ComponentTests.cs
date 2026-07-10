using ComputerCompanyClasses;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace UnitTestProject
{
    [TestClass]
    public class ComponentTests
    {
        //Подключение к БД для тестов
        public static SqliteConnection CreateTestConnection()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db");
            string connectionString = $"Data Source={dbPath};Mode=ReadWriteCreate;Cache=Shared";
            return new SqliteConnection(connectionString);
        }

        [TestMethod]
        public void AddDeleteAndFindCPUInDataBase()
        {
            // Arrange
            CPU cpu = new CPU()
            {
                Model = "Intel Core I9 Ryzen 9900 Ultra X3D",
                Socket = "AM1700",
                Count = 1,
                Price = 100
            };

            // Act
            cpu.AddToDataBase();

            // Assert
            bool result = cpu.FindInDataBase();
            cpu.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateCPUInDataBase()
        {
            // Arrange
            CPU cpu = new CPU()
            {
                Model = "Intel Core I9 Ryzen 9900 Ultra X3D",
                Socket = "AM1700",
                Count = 1,
                Price = 100
            };
            cpu.AddToDataBase();

            // Act
            cpu.Model = "AMD Xeon I7 17900KF";
            cpu.Socket = "LGA 6";
            cpu.Count = 200;
            cpu.Price = 100;
            cpu.UpdateInDataBase();

            // Assert
            bool result = cpu.FindInDataBase();
            cpu.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindGPUInDataBase()
        {
            // Arrange
            GPU gpu = new GPU()
            {
                Model = "General Dynamics X3D",
                ModelCore = "GeForce GTX 580",
                VideoMemory = 14512,
                Count = 1,
                Price = 100
            };

            // Act
            gpu.AddToDataBase();

            // Assert
            bool result = gpu.FindInDataBase();
            gpu.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateGPUInDataBase()
        {
            // Arrange
            GPU gpu = new GPU()
            {
                Model = "General Dynamics X3D",
                ModelCore = "GeForce GTX 580",
                VideoMemory = 14512,
                Count = 1,
                Price = 100
            };
            gpu.AddToDataBase();

            // Act
            gpu.Model = "NoName";
            gpu.ModelCore = "Ryzen ARC 512";
            gpu.VideoMemory = 511;
            gpu.Count = 200;
            gpu.Price = 100;
            gpu.UpdateInDataBase();

            // Assert
            bool result = gpu.FindInDataBase();
            gpu.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindMemoryInDataBase()
        {
            // Arrange
            Memory memory = new Memory()
            {
                Model = "Furry x256",
                Size = 1000,
                Type = "DDR1",
                Count = 1,
                Price = 100
            };

            // Act
            memory.AddToDataBase();

            // Assert
            bool result = memory.FindInDataBase();
            memory.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateMemoryInDataBase()
        {
            // Arrange
            Memory memory = new Memory()
            {
                Model = "Furry x256",
                Size = 1000,
                Type = "DDR1",
                Count = 1,
                Price = 100
            };
            memory.AddToDataBase();

            // Act
            memory.Model = "Kingston x128";
            memory.Size = 128;
            memory.Type = "DDR7";
            memory.Count = 200;
            memory.Price = 100;
            memory.UpdateInDataBase();

            // Assert
            bool result = memory.FindInDataBase();
            memory.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindStorageInDataBase()
        {
            // Arrange
            Storage storage = new Storage()
            {
                Model = "Furry x256",
                Size = 1000,
                Form_factor = "3,5\"",
                Type = "DDR1",
                Count = 1,
                Price = 100
            };

            // Act
            storage.AddToDataBase();

            // Assert
            bool result = storage.FindInDataBase();
            storage.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateStorageInDataBase()
        {
            // Arrange
            Storage storage = new Storage()
            {
                Model = "WD Blue",
                Size = 1000,
                Type = "SSD",
                Form_factor = "3,5\"",
                Count = 1,
                Price = 100
            };
            storage.AddToDataBase();

            // Act
            storage.Model = "Samsung";
            storage.Size = 128;
            storage.Type = "HDD";
            storage.Form_factor = "2,5\"";
            storage.Count = 200;
            storage.Price = 100;
            storage.UpdateInDataBase();

            // Assert
            bool result = storage.FindInDataBase();
            storage.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindPowerUnitInDataBase()
        {
            // Arrange
            Power_unit powerUnit = new Power_unit()
            {
                Model = "KCAS",
                Power = 10000,
                Certification = "90+ Titanium",
                Form_factor = "ATX",
                Count = 1,
                Price = 100
            };

            // Act
            powerUnit.AddToDataBase();

            // Assert
            bool result = powerUnit.FindInDataBase();
            powerUnit.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdatePowerUnitInDataBase()
        {
            // Arrange
            Power_unit powerUnit = new Power_unit()
            {
                Model = "KCAS",
                Power = 10000,
                Certification = "90+ Titanium",
                Form_factor = "ATX",
                Count = 1,
                Price = 100
            };
            powerUnit.AddToDataBase();

            // Act
            powerUnit.Model = "DeepCool";
            powerUnit.Power = 10;
            powerUnit.Certification = "70-";
            powerUnit.Form_factor = "mini ATX";
            powerUnit.Count = 200;
            powerUnit.Price = 100;
            powerUnit.UpdateInDataBase();

            // Assert
            bool result = powerUnit.FindInDataBase();
            powerUnit.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindFrameInDataBase()
        {
            // Arrange
            Frame frame = new Frame()
            {
                Model = "LianLi",
                Form_factor = "mATX",
                Count = 1,
                Price = 100
            };

            // Act
            frame.AddToDataBase();

            // Assert
            bool result = frame.FindInDataBase();
            frame.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateFrameInDataBase()
        {
            // Arrange
            Frame frame = new Frame()
            {
                Model = "KCAS",
                Form_factor = "ATX",
                Count = 1,
                Price = 100
            };
            frame.AddToDataBase();

            // Act
            frame.Model = "PowerGaming";
            frame.Form_factor = "ATX";
            frame.Count = 200;
            frame.Price = 100;
            frame.UpdateInDataBase();

            // Assert
            bool result = frame.FindInDataBase();
            frame.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AddDeleteAndFindMotherboardInDataBase()
        {
            // Arrange
            Motherboard motherboard = new Motherboard()
            {
                Model = "ASRock B650",
                Chipset = "B650",
                Socket = "LGA 1700",
                Count = 1,
                Price = 100
            };

            // Act
            motherboard.AddToDataBase();

            // Assert
            bool result = motherboard.FindInDataBase();
            motherboard.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void UpdateMotherboardInDataBase()
        {
            // Arrange
            Motherboard motherboard = new Motherboard()
            {
                Model = "ASRock B650",
                Chipset = "B650",
                Socket = "LGA 1700",
                Count = 1,
                Price = 100
            };
            motherboard.AddToDataBase();

            // Act
            motherboard.Model = "Asus A250";
            motherboard.Chipset = "A250";
            motherboard.Socket = "AM4";
            motherboard.Count = 200;
            motherboard.Price = 100;
            motherboard.UpdateInDataBase();

            // Assert
            bool result = motherboard.FindInDataBase();
            motherboard.DeleteFromDataBase();
            Assert.AreEqual(true, result);
        }
    }
}
