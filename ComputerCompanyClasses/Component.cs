using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace ComputerCompanyClasses
{
    public class Component : Product
    {
        private long count;

        protected readonly static string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DataBase", "ComputerCompany.db");

        public long Count { get => count; set => count = value; }

        public virtual List<Component> GetComponent()
        {
            throw new Exception("Метод GetComponent доступен только для дочерних классов");
        }
        protected virtual bool ChangeDataBase(string commandText, bool useSelect = false)
        {
            throw new Exception("Метод ChangeDataBase доступен только для дочерних классов");
        }
        public virtual void AddToDataBase()
        {
            throw new Exception("Метод AddToDataBase доступен только для дочерних классов");
        }
        public virtual void UpdateInDataBase()
        {
            throw new Exception("Метод UpdateInDataBase доступен только для дочерних классов");
        }
        public virtual void DeleteFromDataBase()
        {
            throw new Exception("Метод DeleteFromDataBase доступен только для дочерних классов");
        }
        public virtual bool FindInDataBase()
        {
            throw new Exception("Метод DeleteFromDataBase доступен только для дочерних классов");
        }

        public virtual List<DataGridTextColumn> Columns()
        {
            List<DataGridTextColumn> columns = new List<DataGridTextColumn>();
            return columns;
        }
    }
}
