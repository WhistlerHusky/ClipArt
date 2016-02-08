using System;
using System.Windows.Input;

namespace ClipArt
{
    /// <summary>
    ///     Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// </summary>
        public Func<bool> CanExecuteFunc { get; set; }

        /// <summary>
        /// </summary>
        public Action CommandAction { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.CanExecuteFunc == null || this.CanExecuteFunc();
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.CommandAction();
        }
    }
}