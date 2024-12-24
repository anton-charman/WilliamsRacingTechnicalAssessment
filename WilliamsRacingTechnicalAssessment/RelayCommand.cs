using System.Windows.Input;

namespace WilliamsRacingTechnicalAssessment
{
    /// <summary>
    /// Contains boilerplate code for implementing ICommand.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecute)
        {
            _executeMethod = executeMethod;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }     

        public bool CanExecute(object? parameter)
        {
            if(_canExecute != null)
            {
                return _canExecute((T)parameter);
            }

            return _executeMethod != null;
        }

        public void Execute(object? parameter)
        {
            _executeMethod?.Invoke((T)parameter);
        }
    }
}
