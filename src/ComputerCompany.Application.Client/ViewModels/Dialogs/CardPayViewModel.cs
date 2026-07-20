using ComputerCompany.Application.Client.Abstractions.Servies.Dialog;
using ComputerCompany.Application.Client.ViewModels.Commands;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public class CardPayViewModel : BaseDialogViewModel
{
    private readonly IMessageService _messageService;

    private int _first4;
    private int _second4;
    private int _third4;
    private int _fourth4;

    private int _month;
    private int _year;

    private int _cvvcvc;

    private string _name = string.Empty;

    public int First4
    {
        get => _first4;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 4) return;
            ChangeProperty(ref _first4, value);
        }
    }

    public int Second4
    {
        get => _second4;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 4) return;
            ChangeProperty(ref _second4, value);
        }
    }

    public int Third4
    {
        get => _third4;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 4) return;
            ChangeProperty(ref _third4, value);
        }
    }

    public int Fourth4
    {
        get => _fourth4;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 4) return;
            ChangeProperty(ref _fourth4, value);
        }
    }

    public int Year
    {
        get => _year;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 2) return;
            ChangeProperty(ref _year, value);
        }
    }

    public int Month
    {
        get => _month;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 2) return;
            ChangeProperty(ref _month, value);
        }
    }

    public int CvvCvc
    {
        get => _cvvcvc;
        set
        {
            if ((int)Math.Log10(Math.Abs(value)) + 1 > 3) return;
            ChangeProperty(ref _cvvcvc, value);
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            ChangeProperty(ref _name, value);
        }
    }

    public CardPayViewModel(IMessageService messageService)
    {
        _messageService = messageService;

        AcceptCommand = new RelayCommand(Accept);
        DenyCommand = new RelayCommand(() => { DialogResult = false; });
    }

    public void Accept()
    {
        if ((int)Math.Log10(Math.Abs(_cvvcvc)) + 1 < 3)
        {
            _messageService.ShowErrorMessage("Введите CVV/CVC");
            return;
        }
        if
        (
            ((int)Math.Log10(Math.Abs(_first4)) + 1 < 4) ||
            ((int)Math.Log10(Math.Abs(_second4)) + 1 < 4) ||
            ((int)Math.Log10(Math.Abs(_third4)) + 1 < 4) ||
            ((int)Math.Log10(Math.Abs(_fourth4)) + 1 < 4)
        )
        {
            _messageService.ShowErrorMessage("Введите номер карты");
            return;
        }
        if
        (
            ((int)Math.Log10(Math.Abs(_year)) + 1 < 2) ||
            ((int)Math.Log10(Math.Abs(_month)) + 1 < 2)
        )
        {
            _messageService.ShowErrorMessage("Введите дату истечения срока использования");
            return;
        }
        if (Name == string.Empty || Name == null)
        {
            _messageService.ShowErrorMessage("Введите имя латинскими буквами");
            return;
        }
        DialogResult = true;
    }
}