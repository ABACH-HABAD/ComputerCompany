using System.Windows.Input;
using ComputerCompany.Application.Client.Abstractions.ViewModels;

namespace ComputerCompany.Application.Client.ViewModels.Dialogs;

public abstract class BaseDialogViewModel : BaseViewModel, IWindowViewModel, IDialogViewModel
{
    private object? _resultData = null;
    private bool _dialogResult;
    public virtual object? ResultData
    {
        get => _resultData;
        protected set => _resultData = value;
    }
    public virtual bool DialogResult
    {
        get => _dialogResult;
        protected set
        {
            _dialogResult = value;
            Close();
        }
    }

    public virtual ICommand AcceptCommand { get; protected init; } = null!;
    public virtual ICommand DenyCommand { get; protected init; } = null!;


    public event EventHandler<EventArgs>? Closed;

    public virtual void Close()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}