using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerCompanyClasses
{
    public class Assembly
    {
        private CPU usedCPU;
        private GPU usedGPU;
        private Motherboard usedMotherboard;
        private Memory usedMemory;
        private Power_unit usedPowerUnit;
        private Storage usedStorage;
        private Frame usedFrame;
        private int usedMemoryCount;

        public CPU UsedCPU { get => usedCPU; }
        public Motherboard UsedMotherboard { get => usedMotherboard; }
        public Memory UsedMemory { get => usedMemory; }
        public Power_unit UsedPowerUnit { get => usedPowerUnit; }
        public Storage UsedStorage { get => usedStorage; }
        public Frame UsedFrame { get => usedFrame; }
        public GPU UsedGPU { get => usedGPU; }
        public int UsedMemoryCount { get => usedMemoryCount; }

        protected readonly string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");

        public void FindAssemblyInShoppingCart()
        {
            usedCPU = (CPU)ShoppingCart.FindByType(typeof(CPU));
            usedMotherboard = (Motherboard)ShoppingCart.FindByType(typeof(Motherboard));
            usedMemory = (Memory)ShoppingCart.FindByType(typeof(Memory));
            usedPowerUnit = (Power_unit)ShoppingCart.FindByType(typeof(Power_unit));
            usedStorage = (Storage)ShoppingCart.FindByType(typeof(Storage));
            usedFrame = (Frame)ShoppingCart.FindByType(typeof(Frame));
            usedGPU = (GPU)ShoppingCart.FindByType(typeof(GPU));
            usedMemoryCount = ShoppingCart.FindCountByType(typeof(Memory));
        }

        public bool CheckAssemblyCompleteness()
        {
            if (usedCPU == null) throw new Exception("Не выбран процессор");
            if (usedGPU == null) throw new Exception("Не выбрана видеокарта");
            if (usedMemory == null) throw new Exception("Не выбрана оперативная память");
            if (usedMotherboard == null) throw new Exception("Не выбрана материнская плата");
            if (usedPowerUnit == null) throw new Exception("Не выбран блок питания");
            if (usedStorage == null) throw new Exception("Не выбран накопитель (SSD или жесткий диск)");
            if (usedFrame == null) throw new Exception("Не выбран корпус");
            if (usedMemoryCount < 1) throw new Exception("Слишком мало оперативной памяти");

            return true;
        }

        public bool CheckErrorsInAssembly()
        {
            if (usedCPU.Socket != usedMotherboard.Socket) throw new Exception("Сокет на материнской плате не соответствует сокету процессора");
            if (usedFrame.Form_factor != usedPowerUnit.Form_factor) throw new Exception("Блок питания не подходит к корпусу");
            if (usedMemoryCount > 8) throw new Exception("Слишком много оперативной памяти");
            if (usedMemoryCount % 2 != 0) throw new Exception("Количество планок оперативной памяти должно быть чётным");

            return true;
        }

        public void SendInDataBase()
        {
            try
            {
                CheckAssemblyCompleteness();
                CheckErrorsInAssembly();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            long id_pc = 0;
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    SqliteCommand command = new SqliteCommand()
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "INSERT INTO PC\r\n" +
                            "(cpu, gpu, memory, memory_count, motherboard, power_unit, storage, frame)\r\n" +
                            "VALUES\r\n" +
                            "((SELECT id_component FROM CPU WHERE model = @cpuModel),\r\n" +
                            "(SELECT id_component FROM GPU WHERE model = @gpuModelCore AND manufacture = @gpuModel),\r\n" +
                            "(SELECT id_component FROM Memory WHERE model = @memoryModel AND type = @memoryType AND size = @memorySize),\r\n" +
                            "@memoryCount,\r\n" +
                            "(SELECT id_component FROM Motherboard WHERE model = @motherboardModel),\r\n" +
                            "(SELECT id_component FROM Power_unit WHERE model = @powerUnitModel AND power = @powerUnitPower),\r\n" +
                            "(SELECT id_component FROM Storage WHERE model = @storageModel AND size = @storageSize AND type = @storageType),\r\n" +
                            "(SELECT id_component FROM Frame WHERE model = @frameModel))"
                    };
                    command.Parameters.AddWithValue("@cpuModel", usedCPU.Model);
                    command.Parameters.AddWithValue("@gpuModel", usedGPU.Model);
                    command.Parameters.AddWithValue("@gpuModelCore", usedGPU.ModelCore);
                    command.Parameters.AddWithValue("@memoryModel", usedMemory.Model);
                    command.Parameters.AddWithValue("@memoryType", usedMemory.Type);
                    command.Parameters.AddWithValue("@memorySize", usedMemory.Size);
                    command.Parameters.AddWithValue("@memoryCount", usedMemoryCount);
                    command.Parameters.AddWithValue("@motherboardModel", usedMotherboard.Model);
                    command.Parameters.AddWithValue("@powerUnitModel", usedPowerUnit.Model);
                    command.Parameters.AddWithValue("@powerUnitPower", usedPowerUnit.Power);
                    command.Parameters.AddWithValue("@storageModel", usedStorage.Model);
                    command.Parameters.AddWithValue("@storageSize", usedStorage.Size);
                    command.Parameters.AddWithValue("@storageType", usedStorage.Type);
                    command.Parameters.AddWithValue("@frameModel", usedFrame.Model);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    command = new SqliteCommand()
                    {
                        Connection = connection,
                        CommandText = "SELECT id_pc FROM PC\r\n" +
                            "WHERE cpu = (SELECT id_component FROM CPU WHERE model = @cpuModel) AND\r\n" +
                            "gpu = (SELECT id_component FROM GPU WHERE model = @gpuModelCore AND manufacture = @gpuModel) AND\r\n" +
                            "memory = (SELECT id_component FROM Memory WHERE model = @memoryModel AND type = @memoryType AND size = @memorySize) AND\r\n" +
                            "motherboard = (SELECT id_component FROM Motherboard WHERE model = @motherboardModel) AND\r\n" +
                            "power_unit = (SELECT id_component FROM Power_unit WHERE model = @powerUnitModel AND power = @powerUnitPower) AND\r\n" +
                            "storage = (SELECT id_component FROM Storage WHERE model = @storageModel AND size = @storageSize AND type = @storageType) AND\r\n" +
                            "frame = (SELECT id_component FROM Frame WHERE model = @frameModel)"
                    };
                    command.Parameters.AddWithValue("@cpuModel", usedCPU.Model);
                    command.Parameters.AddWithValue("@gpuModel", usedGPU.Model);
                    command.Parameters.AddWithValue("@gpuModelCore", usedGPU.ModelCore);
                    command.Parameters.AddWithValue("@memoryModel", usedMemory.Model);
                    command.Parameters.AddWithValue("@memoryType", usedMemory.Type);
                    command.Parameters.AddWithValue("@memorySize", usedMemory.Size);
                    command.Parameters.AddWithValue("@motherboardModel", usedMotherboard.Model);
                    command.Parameters.AddWithValue("@powerUnitModel", usedPowerUnit.Model);
                    command.Parameters.AddWithValue("@powerUnitPower", usedPowerUnit.Power);
                    command.Parameters.AddWithValue("@storageModel", usedStorage.Model);
                    command.Parameters.AddWithValue("@storageSize", usedStorage.Size);
                    command.Parameters.AddWithValue("@storageType", usedStorage.Type);
                    command.Parameters.AddWithValue("@frameModel", usedFrame.Model);


                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id_pc = (long)reader["id_pc"];
                    }
                }
            }

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {

                    SqliteCommand command = new SqliteCommand()
                    {
                        Connection = connection,
                        Transaction = transaction,
                        CommandText = "INSERT INTO Assembly\r\n" +
                            "(price, pc, client, date, deadline)\r\n" +
                            "VALUES (\r\n" +
                            "@price,\r\n" +
                            "@id_pc,\r\n" +
                            "@client,\r\n" +
                            "date('now'),\r\n" +
                            "date('now', '+30 days'))"
                    };
                    List<Product> list = new List<Product>() { usedCPU, usedGPU, usedStorage, UsedFrame, usedPowerUnit, usedMotherboard };
                    for (int i = 0; i < usedMemoryCount; i++) list.Add(usedMemory);
                    command.Parameters.AddWithValue("@price", ShoppingCart.Total(list));
                    command.Parameters.AddWithValue("@id_pc", id_pc);
                    command.Parameters.AddWithValue("@client", 1);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                }
            }
        }
    }
}
