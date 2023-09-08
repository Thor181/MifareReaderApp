using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace MifareReaderApp.Views
{
    public partial class AdministratorPage : UserControl, IPage
    {
        public AdministratorPage()
        {
            InitializeComponent();
        }

        public void BeforeOpen()
        {
            var dialog = new InputPasswordDialog("Введите пароль");
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }
    }
}
