using System.Media;
using System.Windows;

namespace MifareReaderApp.Views.Dialogs
{
    public partial class MessageDialog : Window
    {
        public string Message { get; private set; }

        private MessageDialog(string message)
        {
            InitializeComponent();
            Owner = App.Current.MainWindow;
            DataContext = this;
            Message = message;
        }

        public static bool? ShowDialog(string message)
        {
            bool? result = false;
            App.DispatcherInvoke(() =>
            {
                var dialog = new MessageDialog(message);

                SystemSounds.Asterisk.Play();

                result = dialog.ShowDialog();
            });

            return result;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
