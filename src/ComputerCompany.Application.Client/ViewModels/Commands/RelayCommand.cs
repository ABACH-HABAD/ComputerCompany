using System.Windows.Input;

namespace ComputerCompany.Application.Client.ViewModels.Commands;

public class RelayCommand(Action commandDelegate, Func<bool>? canExecute = null) : ICommand
{
    private readonly Action _commandDelegate = commandDelegate;
    private readonly Func<bool>? _canExecute = canExecute;

    public event EventHandler? CanExecuteChanged;

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;

        _commandDelegate();
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
}

public class RelayCommand<T>(Action<T> commandDelegate, Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly Action<T> _commandDelegate = commandDelegate;
    private readonly Func<T, bool>? _canExecute = canExecute;

    public event EventHandler? CanExecuteChanged;

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        if (parameter is not T typedParametr) return;

        _commandDelegate(typedParametr);
    }

    public bool CanExecute(object? parameter)
    {
        if (parameter is not T typedParametr) return false;
        return _canExecute == null || _canExecute(typedParametr);
    }
}