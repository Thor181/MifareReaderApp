using MifareReaderApp.Stuff;
using MifareReaderApp.ViewModels;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views.Interfaces;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace MifareReaderApp.Views
{
    public partial class AdministratorPage : UserControl, IPage
    {
        public AdministratorPageViewModel ViewModel { get; set; }

        public AdministratorPage()
        {
            ViewModel = new AdministratorPageViewModel();

            InitializeComponent();
        }

        public void BeforeOpen()
        {
            
        }
    }
}
