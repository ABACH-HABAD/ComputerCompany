using System.Windows.Input;

namespace ComputerCompany.Application.Client.Abstractions.ViewModels;

public interface IDialogViewModel
{
    public object? ResultData { get; }
    public bool DialogResult { get; }

    public ICommand AcceptCommand { get; }
    public ICommand DenyCommand { get; }
}