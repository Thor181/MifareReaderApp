using MifareReaderApp.ViewModels;
using MifareReaderApp.Views;
using System.Windows;

namespace MifareReaderApp
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            ViewModel = new MainWindowViewModel(this);
            InitializeComponent();

            var operatorPageVM = (OperatorTab.Content as OperatorPage)!.ViewModel;
            ViewModel.InitializeViewModels(operatorPageVM);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            ViewModel.InitializePort();

        }
    }
}