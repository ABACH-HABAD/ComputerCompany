using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace ComputerCompanyClasses
{
    public class Frame : Component
    {
        private string model;
        private string form_factor;

        public string Model { get => model; set => model = value; }
        public string Form_factor { get => form_factor; set => form_factor = value; }

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
                    CommandText = "SELECT price, count, model, form_factor FROM Frame"
                };

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add(new Frame
                            {
                                Price = (long)reader["price"],
                                Count = (long)reader["count"],
                                Model = (string)reader["model"],
                                Form_factor = (string)reader["form_factor"],
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
            ChangeDataBase("INSERT INTO Frame (price, count, model, form_factor) VALUES (@price, @count, @model, @form_factor)");
        }

        public override void UpdateInDataBase()
        {
            ChangeDataBase("UPDATE Frame SET price = @price, count = @count, model = @model, form_factor = @form_factor");
        }
        public override void DeleteFromDataBase()
        {
            ChangeDataBase("DELETE FROM Frame WHERE price = @price AND count = @count AND model = @model AND form_factor = @form_factor");
        }
        public override bool FindInDataBase()
        {
            return ChangeDataBase("SELECT * FROM Frame WHERE price = @price AND count = @count AND model = @model AND form_factor = @form_factor AND true", true);
        }

        public override List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = base.Columns();

            columns.Add(new DataGridTextColumn() { Header = "Форм-фактор", Binding = new Binding(path: "Form_factor") });
            columns.Add(new DataGridTextColumn() { Header = "Модель", Binding = new Binding(path: "Model") });

            return columns;
        }
    }
}
