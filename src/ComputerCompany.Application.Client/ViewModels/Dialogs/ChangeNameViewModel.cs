using ComputerCompany.Core.Models;
using ComputerCompany.Application.Client.ViewModels.Commands;
using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class ChangeNameViewModel : BaseDataDialogViewModel<AccountModel>
{
    private readonly IMessageService _messageService;

    private AccountModel _account = null!;

    private string _displayName = string.Empty;

    public string ParametrName { get; } = "Имя:";

    public string ParametrValue
    {
        get => _displayName;
        set
        {
            ChangeProperty(ref _displayName, value);
        }
    }

    public override AccountModel Data => _account with { Name = ParametrValue };


    public ChangeNameViewModel(IMessageService message)
    {
        _messageService = message;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(Declare);
    }

    public override void SetData(AccountModel data)
    {
        _account = data;
        ParametrValue = data.Name;
    }

    private void Accept()
    {
        if (ParametrValue == string.Empty || ParametrValue == null)
        {
            _messageService.ShowErrorMessage("Введите имя");
            return;
        }

        DialogResult = true;
    }

    private void Declare()
    {
        DialogResult = false;
    }
}