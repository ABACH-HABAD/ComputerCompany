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
    public class Storage : Component
    {
        private string model;
        private long size;
        private string type;
        private string form_factor;

        public string Model { get => model; set => model = value; }
        public long Size { get => size; set => size = value; }
        public string Type { get => type; set => type = value; }
        public string Form_factor { get => form_factor; set => form_factor = value; }

        public override string ToString()
        {
            return form_factor+" "+type+ " " + model + " " + size;
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
                    CommandText = "SELECT price, count, model, size, type, form_factor FROM Storage"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Storage
                            {
                                Price = (long)reader["price"],
                                Count = (long)reader["count"],
                                Model = (string)reader["model"],
                                Size = (long)reader["size"],
                                Type = (string)reader["type"],
                                Form_factor = (string)reader["form_factor"]
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
                        command.Parameters.AddWithValue("@form_factor", Form_factor);
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
            ChangeDataBase("INSERT INTO Storage (price, count, model, size, form_factor, type) VALUES (@price, @count, @model, @size, @form_factor, @type)");
        }

        public override void UpdateInDataBase()
        {
            ChangeDataBase("UPDATE Storage SET price = @price, count = @count, model = @model, form_factor = @form_factor, size=size, type=@type");
        }
        public override void DeleteFromDataBase()
        {
            ChangeDataBase("DELETE FROM Storage WHERE price = @price AND count = @count AND model = @model AND form_factor = @form_factor AND size=size AND type=@type");
        }
        public override bool FindInDataBase()
        {
            return ChangeDataBase("SELECT * FROM Storage WHERE price = @price AND count = @count AND model = @model AND form_factor = @form_factor AND size=size AND type=@type", true);
        }
        public override List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = base.Columns();

            columns.Add(new DataGridTextColumn() { Header = "Форм-фактор", Binding = new Binding(path: "Form_factor") });
            columns.Add(new DataGridTextColumn() { Header = "Тип", Binding = new Binding(path: "Type") });
            columns.Add(new DataGridTextColumn() { Header = "Объём (Gb)", Binding = new Binding(path: "Size") });
            columns.Add(new DataGridTextColumn() { Header = "Модель", Binding = new Binding(path: "Model") });

            return columns;
        }
    }
}
