using System.Collections.ObjectModel;

namespace ComputerCompany.Infrastructure.Data.Entities
{
/*
    public class DBType<T> : UniversalDBType where T : BaseComponent, new()
    {
        public DBType(string name) : base(name)
        {
            componentType = typeof(T);

            if (typeof(T).IsSubclassOf(typeof(BaseComponent)))
            {
                UpdateData();
            }
            else throw new Exception("Класс T может быть только дочерним для класса Component");
        }

        public override void UpdateData()
        {
            T t = new T();
            List<BaseComponent> databaseList = t.GetComponent();
            database.Clear();
            foreach (BaseComponent value in databaseList)
            {
                database.Add(value);
            }
        }

        public override BaseComponent GenerateNewObjectOfType()
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
        protected readonly ObservableCollection<BaseComponent> database = new ObservableCollection<BaseComponent>();
        private string name;

        public ObservableCollection<BaseComponent> Database { get => database; }
        public string Name { get => name; set => name = value; }
        public Type ComponentType { get => componentType; }

        public virtual void UpdateData()
        {
            throw new Exception("Вызывается только из дочерних классов");
        }

        public virtual BaseComponent GenerateNewObjectOfType()
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
    */
}