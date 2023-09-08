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
        public string Message { get; private set; }

        public InputPasswordDialog(string message)
        {
            InitializeComponent();

            DataContext = this;
            Message = message;
        }

        public new bool ShowDialog()
        {
            var result = base.ShowDialog();

            return result == true;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            var result = CheckPassword(password);
            if (result == false)
                return;

            this.DialogResult = true;
        }

        private bool CheckPassword(string password)
        {
            var result = Password.Check(password);

            if (!result.IsSuccess)
            {
                new MessageDialog(result.Message).ShowDialog();
                return false;
            }


            return true;
        }

    }
}
