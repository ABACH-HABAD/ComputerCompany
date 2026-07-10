using ComputerCompanyClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerCompany
{
    /// <summary>
    /// Логика взаимодействия для ManagerDataBasePage.xaml
    /// </summary>
    public partial class ManagerDataBasePage : Page
    {
        private readonly ObservableCollection<ComputerCompanyClasses.Component> boundDataBase = [];

        public ObservableCollection<ComputerCompanyClasses.Component> BoundDataBase { get => boundDataBase; }

        public ManagerDataBasePage()
        {
            InitializeComponent();

            DataBaseSelector.Items.Add(new DBType<CPU>("CPU"));
            DataBaseSelector.Items.Add(new DBType<GPU>("GPU"));
            DataBaseSelector.Items.Add(new DBType<Motherboard>("Motherboard"));
            DataBaseSelector.Items.Add(new DBType<Memory>("Memory"));
            DataBaseSelector.Items.Add(new DBType<Storage>("Storage"));
            DataBaseSelector.Items.Add(new DBType<Power_unit>("Power_unit"));
            DataBaseSelector.Items.Add(new DBType<ComputerCompanyClasses.Frame>("Frame"));

            DataContext = this;
        }

        private void DataBaseSelector_DropDownClosed(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            if (DataBaseSelector.SelectedItem == null) return;

            boundDataBase.Clear();

            UniversalDBType dataBaseSelector = ((UniversalDBType)(DataBaseSelector.SelectedItem));
            dataBaseSelector.UpdateData();

            foreach (ComputerCompanyClasses.Component component in dataBaseSelector.Database)
            {
                dynamic converted = component;
                boundDataBase.Add(converted);
            }

            while (DataBaseGrid.Columns.Count > 4)
            {
                DataBaseGrid.Columns.RemoveAt(0);
            }

            foreach (DataGridTextColumn column in dataBaseSelector.GetColumns())
            {
                DataBaseGrid.Columns.Insert(0, column);
            }

            DataContext = this;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataBaseSelector.SelectedItem == null) return;
                ComputerCompanyClasses.Component addingComponent = ((UniversalDBType)(DataBaseSelector.SelectedItem)).GenerateNewObjectOfType();

                DataBaseAddDataWindow dataBaseAddDataWindow = new(DataBaseGrid.Columns, addingComponent);
                if (dataBaseAddDataWindow.ShowDialog() == true)
                {
                    addingComponent.AddToDataBase();
                    MessageBox.Show("Успешно добавлено в базу данных");
                    UpdateData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is ComputerCompanyClasses.Component component)
                {
                    try
                    {
                        DataBaseAddDataWindow dataBaseAddDataWindow = new(DataBaseGrid.Columns, component, updateMode: true);
                        if (dataBaseAddDataWindow.ShowDialog() == true)
                        {
                            component.UpdateInDataBase();
                            MessageBox.Show("Успешно изменено в базе данных");
                            UpdateData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is ComputerCompanyClasses.Component component)
                {
                    if (MessageBox.Show("Вы точно хотите удалить?", "Вы точно хотите удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            component.DeleteFromDataBase();
                            MessageBox.Show("Успешно удалено из базы данных");
                            UpdateData();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
