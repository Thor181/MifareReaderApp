using System.Media;
using System.Windows;

namespace MifareReaderApp.Views.Dialogs
{
    public partial class MessageDialog : Window
    {
        public string Message { get; private set; }

        public MessageDialog(string message)
        {
            InitializeComponent();
            Owner = App.Current.MainWindow;
            DataContext = this;
            Message = message;
        }

        public new bool? ShowDialog()
        {
            SystemSounds.Asterisk.Play();
            
            return base.ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
