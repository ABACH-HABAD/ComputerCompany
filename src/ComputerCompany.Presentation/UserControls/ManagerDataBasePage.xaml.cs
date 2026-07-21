using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComputerCompany.Presentation.UserControls;

/// <summary>
/// Логика взаимодействия для ManagerDataBaseUserControl.xaml
/// </summary>
public partial class ManagerDataBaseUserControl : UserControl
{
    private const int InsertPosition = 5;

    private const string Cpu = "Процессор";
    private const string Gpu = "Видеокарта";
    private const string Motherboard = "Материнская плата";
    private const string Memory = "Оперативная память";
    private const string Storage = "Накопитель";
    private const string PowerUnit = "Блок питания";
    private const string Frame = "Корпус";

    private readonly DataGridTextColumn _socketColumn = new() { Header = "Сокет", Binding = new Binding() { Path = new PropertyPath("Socket") } };
    private readonly DataGridTextColumn _modelCoreColumn = new() { Header = "Видеоядро", Binding = new Binding() { Path = new PropertyPath("ModelCore") } };
    private readonly DataGridTextColumn _videoMemoryColumn = new() { Header = "Видеопамять", Binding = new Binding() { Path = new PropertyPath("VideoMemory") } };
    private readonly DataGridTextColumn _chipsetColumn = new() { Header = "Чипсет", Binding = new Binding() { Path = new PropertyPath("Chipset") } };
    private readonly DataGridTextColumn _sizeColumn = new() { Header = "Объём", Binding = new Binding() { Path = new PropertyPath("Size") } };
    private readonly DataGridTextColumn _typeColumn = new() { Header = "Тип", Binding = new Binding() { Path = new PropertyPath("Type") } };
    private readonly DataGridTextColumn _frequencyColumn = new() { Header = "Частота", Binding = new Binding() { Path = new PropertyPath("Frequency") } };
    private readonly DataGridTextColumn _formFactorColumn = new() { Header = "Форм фактор", Binding = new Binding() { Path = new PropertyPath("FormFactor") } };
    private readonly DataGridTextColumn _powerColumn = new() { Header = "Мощность", Binding = new Binding() { Path = new PropertyPath("Power") } };
    private readonly DataGridTextColumn _certificationColumn = new() { Header = "Сертификат", Binding = new Binding() { Path = new PropertyPath("Certification") } };

    private string _lastSelectedItem = string.Empty;

    public ManagerDataBaseUserControl() { InitializeComponent(); }

    private void DataBaseSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            string currnetElement = (comboBox.SelectedItem as string)!;

            if (_lastSelectedItem == currnetElement) return;

            if (_lastSelectedItem != string.Empty)
            {
                switch (_lastSelectedItem)
                {
                    case Cpu:
                        ComponentsDataGrid.Columns.Remove(_socketColumn);
                        break;
                    case Gpu:
                        ComponentsDataGrid.Columns.Remove(_videoMemoryColumn);
                        ComponentsDataGrid.Columns.Remove(_modelCoreColumn);
                        break;
                    case Motherboard:
                        ComponentsDataGrid.Columns.Remove(_chipsetColumn);
                        ComponentsDataGrid.Columns.Remove(_socketColumn);
                        break;
                    case Memory:
                        ComponentsDataGrid.Columns.Remove(_typeColumn);
                        ComponentsDataGrid.Columns.Remove(_sizeColumn);
                        ComponentsDataGrid.Columns.Remove(_frequencyColumn);
                        break;
                    case Storage:
                        ComponentsDataGrid.Columns.Remove(_typeColumn);
                        ComponentsDataGrid.Columns.Remove(_sizeColumn);
                        ComponentsDataGrid.Columns.Remove(_formFactorColumn);
                        break;
                    case PowerUnit:
                        ComponentsDataGrid.Columns.Remove(_powerColumn);
                        ComponentsDataGrid.Columns.Remove(_formFactorColumn);
                        ComponentsDataGrid.Columns.Remove(_certificationColumn);
                        break;
                    case Frame:
                        ComponentsDataGrid.Columns.Remove(_formFactorColumn);
                        break;
                    default: break;
                }
            }

            switch (comboBox.SelectedItem as string)
            {
                case Cpu:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _socketColumn);
                    break;
                case Gpu:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _videoMemoryColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _modelCoreColumn);
                    break;
                case Motherboard:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _chipsetColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _socketColumn);
                    break;
                case Memory:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _typeColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _sizeColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _frequencyColumn);
                    break;
                case Storage:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _typeColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _sizeColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _formFactorColumn);
                    break;
                case PowerUnit:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _powerColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _formFactorColumn);
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _certificationColumn);
                    break;
                case Frame:
                    ComponentsDataGrid.Columns.Insert(InsertPosition, _formFactorColumn);
                    break;
                default: return;
            }

            _lastSelectedItem = currnetElement;
        }
    }
}