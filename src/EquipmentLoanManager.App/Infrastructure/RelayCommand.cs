using System.Windows.Input;

namespace EquipmentLoanManager.App.Infrastructure;

/// <summary>A command that runs the given action.</summary>
public sealed class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>Checks whether the command can run.</summary>
    public bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

    /// <summary>Runs the command.</summary>
    public void Execute(object? parameter) => execute();
}
