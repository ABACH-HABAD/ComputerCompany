using System.Windows.Input;
using ComputerCompany.Application.Client.Abstractions.ViewModels;
using ComputerCompany.Application.Client.ViewModels.Commands;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class InformationViewModel : BaseDialogViewModel, IDataRequireableViewModel, IDataRequireableViewModel<string>
{
    private string _text = string.Empty;
    public string DisplayText
    {
        get => _text;
        set
        {
            ChangeProperty(ref _text, value);
        }
    }

    public override bool DialogResult => true;

    public InformationViewModel()
    {
        AcceptCommand = new RelayCommand(Close);
        DenyCommand = new RelayCommand(Close);
    }

    public void LoadData(params object[] data)
    {
        foreach (object item in data)
        {
            if (item is string informationString) LoadData(informationString);
        }
    }

    public void LoadData(string data)
    {
        DisplayText = data;
    }
}