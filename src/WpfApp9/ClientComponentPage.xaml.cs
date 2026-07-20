using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ComputerCompanyClasses
{
    /// <summary>
    /// Логика взаимодействия для ClientComponentUserControl.xaml
    /// </summary>
    public partial class ClientComponentUserControl : UserControl
    {
        private static readonly ObservableCollection<ComputerCompanyClasses.Component> boundDataBase = [];

        public static ObservableCollection<ComputerCompanyClasses.Component> BoundDataBase { get => boundDataBase; }

        public ClientComponentUserControl()
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

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (button?.DataContext is ComputerCompanyClasses.Component component)
                {
                    component.AddToShoppingCart();
                }
            }
        }
    }
}
