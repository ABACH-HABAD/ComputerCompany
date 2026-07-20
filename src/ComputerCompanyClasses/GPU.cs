using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComputerCompanyClasses
{
    public class GPU : Component
    {
        private string modelCore;
        private string model;
        private long videoMemory;

        public string ModelCore { get => modelCore; set => modelCore = value; }
        public string Model { get => model; set => model = value; }
        public long VideoMemory { get => videoMemory; set => videoMemory = value; }

        public override string ToString()
        {
            return modelCore + " " + model;
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
                    CommandText = "SELECT price, count, model, manufacture, videomemory FROM GPU"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new GPU()
                            {
                                Price = (long)reader["price"],
                                Count = (long)reader["count"],
                                ModelCore = (string)reader["model"],
                                Model = (string)reader["manufacture"],
                                VideoMemory = (long)reader["videomemory"]
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
                        command.Parameters.AddWithValue("@model", ModelCore);
                        command.Parameters.AddWithValue("@videomemory", VideoMemory);
                        command.Parameters.AddWithValue("@manufacture", Model);
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
            ChangeDataBase("INSERT INTO GPU (price, count, model, videomemory, manufacture) VALUES (@price, @count, @model, @videomemory, @manufacture)");
        }

        public override void UpdateInDataBase()
        {
            ChangeDataBase("UPDATE GPU SET price = @price, count = @count, model = @model, videomemory = @videomemory, manufacture = @manufacture");
        }
        public override void DeleteFromDataBase()
        {
            ChangeDataBase("DELETE FROM GPU WHERE price = @price AND count = @count AND model = @model AND videomemory = @videomemory AND manufacture = @manufacture");
        }
        public override bool FindInDataBase()
        {
            return ChangeDataBase("SELECT * FROM GPU WHERE price = @price AND count = @count AND model = @model AND videomemory = @videomemory AND manufacture = @manufacture", true);
        }

        public override List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = base.Columns();

            columns.Add(new DataGridTextColumn() { Header = "Видеопамять (Mb)", Binding = new Binding(path: "VideoMemory") });
            columns.Add(new DataGridTextColumn() { Header = "Исполнение", Binding = new Binding(path: "Model") });
            columns.Add(new DataGridTextColumn() { Header = "Видеочип", Binding = new Binding(path: "ModelCore") });

            return columns;
        }
    }
}
