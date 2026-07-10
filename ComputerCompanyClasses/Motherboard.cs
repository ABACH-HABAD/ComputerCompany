using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComputerCompanyClasses
{
    public class Motherboard : Component
    {
        private string model;
        private string chipset;
        private string socket;

        public string Model { get => model; set => model = value; }
        public string Socket { get => socket; set => socket = value; }
        public string Chipset { get => chipset; set => chipset = value; }
        public override string ToString()
        {
            return model;
        }
        public override List<Component> GetComponent()
        {
            List<Component> result = new List<Component>();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand()
                {
                    Connection = connection,
                    CommandText = "SELECT price, count, model, chipset, socket FROM Motherboard"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Motherboard()
                            {
                                Price = (long)reader["price"],
                                Count = (long)reader["count"],
                                Model = (string)reader["model"],
                                Chipset = (string)reader["chipset"],
                                Socket = (string)reader["socket"]
                            });
                        }
                    }
                }
            }

            return result;
        }

        protected override bool ChangeDataBase(string commandText, bool useSelect = false)
        {
            using (SqliteConnection connection = new SqliteConnection($"Data Source={dbPath}"))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqliteCommand command = new SqliteCommand()
                        {
                            Connection = connection,
                            Transaction = transaction,
                            CommandText = commandText
                        };
                        command.Parameters.AddWithValue("@price", Price);
                        command.Parameters.AddWithValue("@count", Count);
                        command.Parameters.AddWithValue("@model", Model);
                        command.Parameters.AddWithValue("@socket", Socket);
                        command.Parameters.AddWithValue("@chipset", Chipset);

                        if (useSelect)
                        {
                            using (SqliteDataReader reader = command.ExecuteReader())
                            {
                                return reader.HasRows;
                            }
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public override void AddToDataBase()
        {
            ChangeDataBase("INSERT INTO Motherboard (price, count, model, socket, chipset) VALUES (@price, @count, @model, @socket, @chipset)");
        }

        public override void UpdateInDataBase()
        {
            ChangeDataBase("UPDATE Motherboard SET price = @price, count = @count, model = @model, socket = @socket, chipset = @chipset");
        }
        public override void DeleteFromDataBase()
        {
            ChangeDataBase("DELETE FROM Motherboard WHERE price = @price AND count = @count AND model = @model AND socket = @socket AND chipset = @chipset");
        }
        public override bool FindInDataBase()
        {
            return ChangeDataBase("SELECT * FROM Motherboard WHERE price = @price AND count = @count AND model = @model AND socket = @socket AND chipset = @chipset", true);
        }
        public override List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = base.Columns();

            columns.Add(new DataGridTextColumn() { Header = "Сокет", Binding = new Binding(path: "Socket") });
            columns.Add(new DataGridTextColumn() { Header = "Чипсет", Binding = new Binding(path: "Chipset") });
            columns.Add(new DataGridTextColumn() { Header = "Модель", Binding = new Binding(path: "Model") });

            return columns;
        }
    }
}
