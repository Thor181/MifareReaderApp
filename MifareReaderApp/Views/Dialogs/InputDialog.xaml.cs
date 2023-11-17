using System.Windows;

namespace MifareReaderApp.Views.Dialogs
{
    public partial class InputDialog : Window
    {
        public string Message { get; private set; }
        public string InputString { get; set; }

        public InputDialog(string message)
        {
            Message = message;
            InitializeComponent();
            Owner = App.Current.MainWindow;
        }

        public new string ShowDialog()
        {
            base.ShowDialog();

            return InputString;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
