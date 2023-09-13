using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MifareReaderApp.Stuff.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public delegate void SimpleCommandHandler(object? entity);
        public SimpleCommandHandler CommandHandler { get; set; }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            CommandHandler?.Invoke(parameter);
        }
    }
}
