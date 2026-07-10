using ComputerCompanyClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Shapes;

namespace ComputerCompany
{
    /// <summary>
    /// Логика взаимодействия для DataBaseAddDataWindow.xaml
    /// </summary>
    public partial class DataBaseAddDataWindow : Window
    {
        private readonly Component component;
        private readonly bool updateMode = false;

        private static readonly Dictionary<string, string> fields = new()
        {
            {"Модель", "Model" },
            {"Сокет", "Socket" },
            {"Видеопамять (Mb)", "VideoMemory" },
            {"Исполнение", "Model" },
            {"Видеочип", "ModelCore" },
            {"Частота (Hz)", "Frequency" },
            {"Объём (Mb)", "Size" },
            {"Объём (Gb)", "Size" },
            {"Тип", "Type" },
            {"Форм-фактор", "Form_factor" },
            {"Мощность (Wt)", "Power" },
            {"Сертификация", "Certification" },
            {"Чипсет", "Chipset" }
        };

        public Component ThisComponent { get => component; }

        public DataBaseAddDataWindow(ObservableCollection<DataGridColumn> columns, Component component, bool updateMode = false)
        {
            InitializeComponent();

            this.updateMode = updateMode;
            this.component = component ?? throw new Exception("Если вы НЕ использовали updateMode, то в качестве аргумента component используйте новый объект нужного вам класса");

           Title = updateMode ? "Изменение данных" : "Добавление данных";

            foreach (DataGridColumn column in columns)
            {
                if (column is not DataGridTextColumn) continue;

                string? headerName = column.Header.ToString();
                headerName ??= string.Empty;

                if (fields.TryGetValue(headerName, out string? field))
                {
                    if (AllFields.FindName(field) != null)
                    {
                        Grid grid = (Grid)AllFields.FindName(field);
                        grid.Visibility = Visibility.Visible;
                        grid.Margin = new Thickness(5);
                        grid.Height = 23;
                    }
                }
            }

            LoadData();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            foreach (Grid grid in AllFields.Children)
            {
                if (grid.Visibility == Visibility.Hidden) continue;
                foreach (object obj in grid.Children)
                {
                    if (obj is TextBox textBox)
                    {
                        if (textBox.Text.Length == 0)
                        {
                            MessageBox.Show($"Поле {textBox.Name} не должно быть пустым");
                            return;
                        }
                    }
                }
            }

            if (component.GetType() == typeof(CPU))
            {
                try
                {
                    CPU cpu = (CPU)component;
                    cpu.Socket = SocketText.Text;
                    cpu.Model = ModelText.Text;
                    cpu.Price = long.Parse(PriceText.Text);
                    cpu.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }

            }
            else if (component.GetType() == typeof(GPU))
            {
                try
                {
                    GPU gpu = (GPU)component;
                    gpu.ModelCore = ModelText.Text;
                    gpu.ModelCore = ModelCoreText.Text;
                    gpu.VideoMemory = long.Parse(VideomemoryText.Text);
                    gpu.Price = long.Parse(PriceText.Text);
                    gpu.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(Memory))
            {
                try
                {
                    Memory memory = (Memory)component;
                    memory.Frequency = long.Parse(FrequencyText.Text);
                    memory.Size = long.Parse(SizeText.Text);
                    memory.Type = TypeText.Text;
                    memory.Model = ModelText.Text;
                    memory.Price = long.Parse(PriceText.Text);
                    memory.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(Storage))
            {
                try
                {
                    Storage storage = (Storage)component;
                    storage.Form_factor = FromFactorText.Text;
                    storage.Size = long.Parse(SizeText.Text);
                    storage.Type = TypeText.Text;
                    storage.Model = ModelText.Text;
                    storage.Price = long.Parse(PriceText.Text);
                    storage.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(Power_unit))
            {
                try
                {
                    Power_unit power_unit = (Power_unit)component;
                    power_unit.Form_factor = FromFactorText.Text;
                    power_unit.Certification = CertificationText.Text;
                    power_unit.Power = long.Parse(PowerText.Text);
                    power_unit.Model = ModelText.Text;
                    power_unit.Price = long.Parse(PriceText.Text);
                    power_unit.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(ComputerCompanyClasses.Frame))
            {
                try
                {
                    ComputerCompanyClasses.Frame frame = (ComputerCompanyClasses.Frame)component;
                    frame.Form_factor = FromFactorText.Text;
                    frame.Model = ModelText.Text;
                    frame.Price = long.Parse(PriceText.Text);
                    frame.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(ComputerCompanyClasses.Frame))
            {
                try
                {
                    ComputerCompanyClasses.Frame frame = (ComputerCompanyClasses.Frame)component;
                    frame.Form_factor = FromFactorText.Text;
                    frame.Model = ModelText.Text;
                    frame.Price = long.Parse(PriceText.Text);
                    frame.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }
            else if (component.GetType() == typeof(Motherboard))
            {
                try
                {
                    Motherboard motherboard = (Motherboard)component;
                    motherboard.Model = ModelText.Text;
                    motherboard.Chipset = ChipsetText.Text;
                    motherboard.Socket = SocketText.Text;
                    motherboard.Price = long.Parse(PriceText.Text);
                    motherboard.Count = long.Parse(CountText.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                    return;
                }
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void LoadData()
        {
            PriceText.Text = $"{component.Price}";
            CountText.Text = $"{component.Count}";

            if (component.GetType() == typeof(CPU))
            {
                try
                {
                    CPU cpu = (CPU)component;
                    SocketText.Text = cpu.Socket;
                    ModelText.Text = cpu.Model;
                }
                catch { }

            }
            else if (component.GetType() == typeof(GPU))
            {
                try
                {
                    GPU gpu = (GPU)component;
                    ModelText.Text = gpu.ModelCore;
                    ModelCoreText.Text = gpu.ModelCore;
                    VideomemoryText.Text = $"{gpu.VideoMemory}";
                }
                catch { }
            }
            else if (component.GetType() == typeof(Memory))
            {
                try
                {
                    Memory memory = (Memory)component;
                    FrequencyText.Text = $"{memory.Frequency}";
                    SizeText.Text = $"{memory.Size}";
                    TypeText.Text = memory.Type;
                    ModelText.Text = memory.Model;
                }
                catch { }
            }
            else if (component.GetType() == typeof(Storage))
            {
                try
                {
                    Storage storage = (Storage)component;
                    FromFactorText.Text = storage.Form_factor;
                    SizeText.Text = $"{storage.Size}";
                    TypeText.Text = storage.Type;
                    ModelText.Text = storage.Model;
                }
                catch { }
            }
            else if (component.GetType() == typeof(Power_unit))
            {
                try
                {
                    Power_unit power_unit = (Power_unit)component;
                    FromFactorText.Text = power_unit.Form_factor;
                    CertificationText.Text = power_unit.Certification;
                    PowerText.Text = $"{power_unit.Power}";
                    ModelText.Text = power_unit.Model;
                }
                catch { }
            }
            else if (component.GetType() == typeof(ComputerCompanyClasses.Frame))
            {
                try
                {
                    ComputerCompanyClasses.Frame frame = (ComputerCompanyClasses.Frame)component;
                    FromFactorText.Text = frame.Form_factor;
                    ModelText.Text = frame.Model;
                }
                catch { }
            }
            else if (component.GetType() == typeof(ComputerCompanyClasses.Frame))
            {
                try
                {
                    ComputerCompanyClasses.Frame frame = (ComputerCompanyClasses.Frame)component;
                    FromFactorText.Text = frame.Form_factor;
                    ModelText.Text = frame.Model;
                }
                catch { }
            }
            else if (component.GetType() == typeof(Motherboard))
            {
                try
                {
                    Motherboard motherboard = (Motherboard)component;
                    ModelText.Text = motherboard.Model;
                    ChipsetText.Text = motherboard.Chipset;
                    SocketText.Text = motherboard.Socket;
                }
                catch { }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить?", "Вы точно хотите удалить?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (updateMode)
                {
                    component.DeleteFromDataBase();
                    Close();
                }
                else DialogResult = false;
            }
        }
    }
}
