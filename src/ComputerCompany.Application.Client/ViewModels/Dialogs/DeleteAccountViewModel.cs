using System.Windows.Input;
using ComputerCompany.Application.Client.ViewModels.Commands;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class DeleteAccountViewModel : BaseDialogViewModel
{
    public DeleteAccountViewModel()
    {
        AcceptCommand = new RelayCommand(() =>  DialogResult = true);
        DenyCommand = new RelayCommand(() => DialogResult = false);
    }
}