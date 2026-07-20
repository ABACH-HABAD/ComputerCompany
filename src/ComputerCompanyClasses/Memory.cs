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
    public class Memory : Component
    {
        private string model;
        private long size;
        private string type;
        private long frequency;

        public string Model { get => model; set => model = value; }
        public long Size { get => size; set => size = value; }
        public string Type { get => type; set => type = value; }
        public long Frequency { get => frequency; set => frequency = value; }

        public override string ToString()
        {
            return model+ " " + size + " " + type;
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
                    CommandText = "SELECT price, count, model, size, type, frequency FROM Memory"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Memory
                            {
                                Price = (long)reader["price"],
                                Count = (long)reader["count"],
                                Model = (string)reader["model"],
                                Size = (long)reader["size"],
                                Type = (string)reader["type"],
                                Frequency = (long)reader["frequency"]
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
                        command.Parameters.AddWithValue("@size", Size);
                        command.Parameters.AddWithValue("@type", Type);
                        command.Parameters.AddWithValue("@frequency", Frequency);

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
            ChangeDataBase("INSERT INTO Memory (price, count, model, size, frequency, type) VALUES (@price, @count, @model, @size, @frequency, @type)");
        }

        public override void UpdateInDataBase()
        {
            ChangeDataBase("UPDATE Memory SET price = @price, count = @count, model = @model, frequency = @frequency, size=size, type=@type");
        }
        public override void DeleteFromDataBase()
        {
            ChangeDataBase("DELETE FROM Memory WHERE price = @price AND count = @count AND model = @model AND frequency = @frequency AND size=size AND type=@type");
        }
        public override bool FindInDataBase()
        {
            return ChangeDataBase("SELECT * FROM Memory WHERE price = @price AND count = @count AND model = @model AND frequency = @frequency AND size=size AND type=@type", true);
        }

        public override List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = base.Columns();

            columns.Add(new DataGridTextColumn() { Header = "Частота (Hz)", Binding = new Binding(path: "Frequency") });
            columns.Add(new DataGridTextColumn() { Header = "Тип", Binding = new Binding(path: "Type") });
            columns.Add(new DataGridTextColumn() { Header = "Объём (Mb)", Binding = new Binding(path: "Size") });
            columns.Add(new DataGridTextColumn() { Header = "Модель", Binding = new Binding(path: "Model") });

            return columns;
        }
    }
}
