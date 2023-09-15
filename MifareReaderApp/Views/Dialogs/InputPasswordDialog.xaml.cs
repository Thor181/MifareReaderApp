using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MifareReaderApp.Views.Dialogs
{
    public partial class InputPasswordDialog : Window
    {
        public enum DialogAction
        {
            CheckPassword,
            SetPassword
        }

        private Dictionary<DialogAction, Func<string, bool>> _actionsHandlersMap = new();

        public DialogAction CurrentDialogAction { get; set; }
        public string Message { get; private set; }

        public InputPasswordDialog(string message, DialogAction action)
        {
            InitializeComponent();

            _actionsHandlersMap.Add(DialogAction.CheckPassword, CheckPassword);
            _actionsHandlersMap.Add(DialogAction.SetPassword, SetPassword);

            Owner = App.Current.MainWindow;
            DataContext = this;
            Message = message;
            CurrentDialogAction = action;
        }

        /// <summary>
        /// Показать диалоговое окно <see cref="InputPasswordDialog"/> с проверкой введенного пароля
        /// </summary>
        /// <returns><see cref="true"/>  - если введен верный пароль, иначе <see cref="false"/></returns>
        public new bool ShowDialog()
        {
            var result = base.ShowDialog();

            return result == true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _actionsHandlersMap[CurrentDialogAction].Invoke(PasswordBox.Password);

            //var result = CheckPassword(PasswordBox.Password);
            
            this.DialogResult = result;
        }

        private bool CheckPassword(string password)
        {
            var result = Password.Check(password);

            if (!result.IsSuccess)
            {
                MessageDialog.ShowDialog(result.Message);
                return false;
            }

            return true;
        }

        private bool SetPassword(string password)
        {
            var result = Password.HashAndWriteToFile(password);
            if (!result.IsSuccess)
            {
                MessageDialog.ShowDialog(result.Message); 
                return false;
            }

            return true;
        }

    }
}
