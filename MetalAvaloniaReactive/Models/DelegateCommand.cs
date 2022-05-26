using System;
using System.Windows.Input;

namespace AvaloniaClientMVVM.Models;

public class DelegateCommand : ICommand
{
    /*private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;
 
    public DelegateCommand(Action<T> execute)
        : this(execute, null)
    {
    }
 
    public DelegateCommand(Action<T> execute, Predicate<T> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }
 
    public bool CanExecute(object parameter)
    {
        if (_canExecute == null)
            return true;
 
        return _canExecute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
    }
 
    public void Execute(object parameter)
    {
        _execute((parameter == null) ? default(T) : (T)Convert.ChangeType(parameter, typeof(T)));
    }
 
    public event EventHandler CanExecuteChanged;
    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged != null)
            CanExecuteChanged(this, EventArgs.Empty);
    }*/
    private Action<object> action;
    private Predicate<object> canExecut;
 
    public DelegateCommand(Action<object> action, Predicate<object> canExecut)
    {
        this.action = action != null
            ? action
            : throw new ArgumentNullException();
        this.canExecut = canExecut;
    }
 
    public DelegateCommand(Action<object> action)
        : this(action, null)
    {
    }
 
    public bool CanExecute(object parameter)
    {
        if (parameter == null) return true;
        var t = canExecut?.Invoke((object)parameter) ?? true;
        return t;
    }
 
    public void Execute(object parameter)
    {
        action((object)parameter);
    }
 
    public event EventHandler CanExecuteChanged;
}