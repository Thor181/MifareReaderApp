using MifareReaderApp.Stuff;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views.Interfaces;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace MifareReaderApp.Views
{
    public partial class AdministratorPage : UserControl, IPage
    {
        public Visibility VisibilityProp { get; set; }

        public AdministratorPage()
        {
            VisibilityProp = Visibility.Collapsed;

            InitializeComponent();
        }

        public void BeforeOpen()
        {
            if (AppConfig.Instance.PasswordSetted)
            {
                var dialog = new InputPasswordDialog("Введите пароль");
                var result = dialog.ShowDialog();
                VisibilityProp = result == true ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                MessageDialog.ShowDialog("Пароль не установлен");
            }
        }
    }
}
