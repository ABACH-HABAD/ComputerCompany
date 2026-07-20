using System.Windows.Input;

namespace ComputerCompany.Application.Client.ViewModels.Commands;

public class AsyncRelayCommand(Func<Task> commandDelegate, Func<bool>? canExecute = null) : ICommand
{
    private readonly Func<Task> _commandDelegate = commandDelegate;
    private readonly Func<bool>? _canExecute = canExecute;

    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged;

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;

        try
        {
            _isExecuting = true;

            await _commandDelegate();
        }
        finally
        {
            _isExecuting = false;
        }
    }

    public bool CanExecute(object? parameter) => (_canExecute == null || _canExecute()) && !_isExecuting;
}

public class AsyncRelayCommand<T>(Func<T, Task> commandDelegate, Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly Func<T, Task> _commandDelegate = commandDelegate;
    private readonly Func<T, bool>? _canExecute = canExecute;

    private bool _isExecuting;

    public event EventHandler? CanExecuteChanged;

    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter)) return;
        if (parameter is not T typedParametr) return;

        try
        {
            _isExecuting = true;

            await _commandDelegate(typedParametr);
        }
        finally
        {
            _isExecuting = false;
        }
    }

    public bool CanExecute(object? parameter)
    {
        if (parameter is not T typedParametr) return false;
        return _canExecute == null || _canExecute(typedParametr) && !_isExecuting;
    }
}