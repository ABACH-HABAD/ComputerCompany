using ComputerCompany.Application.Client.ViewModels.Commands;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class QrViewModel : BaseDialogViewModel
{
    public QrViewModel()
    {
        AcceptCommand = new RelayCommand(Close);
        DenyCommand = new RelayCommand(Close);
    }
}