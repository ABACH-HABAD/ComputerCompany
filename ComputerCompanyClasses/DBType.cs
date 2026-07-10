using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerCompanyClasses
{
    public class DBType<T> : UniversalDBType where T : Component, new()
    {
        public DBType(string name) : base(name)
        {
            componentType = typeof(T);

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                UpdateData();
            }
            else throw new Exception("Класс T может быть только дочерним для класса Component");
        }

        public override void UpdateData()
        {
            T t = new T();
            List<Component> databaseList = t.GetComponent();
            database.Clear();
            foreach (Component value in databaseList)
            {
                database.Add(value);
            }
        }

        public override Component GenerateNewObjectOfType()
        {
            return new T();
        }

        public override List<DataGridTextColumn> GetColumns()
        {
            return new T().Columns();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class UniversalDBType
    {
        protected Type componentType;
        protected readonly ObservableCollection<Component> database = new ObservableCollection<Component>();
        private string name;

        public ObservableCollection<Component> Database { get => database; }
        public string Name { get => name; set => name = value; }
        public Type ComponentType { get => componentType; }

        public virtual void UpdateData()
        {
            throw new Exception("Вызывается только из дочерних классов");
        }

        public virtual Component GenerateNewObjectOfType()
        {
            throw new Exception("Вызывается только из дочерних классов");
        }

        public UniversalDBType(string name)
        {
            this.name = name;
        }

        public virtual List<DataGridTextColumn> GetColumns()
        {
            throw new Exception("Вызывается только из дочерних классов");
        }
    }
}
