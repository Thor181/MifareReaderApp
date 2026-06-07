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
        public SimpleCommandHandler CommandHandler { get; set; }

        public delegate void SimpleCommandHandler(object? entity);
        public event EventHandler? CanExecuteChanged;

        public SimpleCommand()
        {
                
        }

        public SimpleCommand(SimpleCommandHandler handler)
        {
            CommandHandler = handler;
        }

        public virtual void Execute(object? parameter)
        {
            CommandHandler?.Invoke(parameter);
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }
    }
}
