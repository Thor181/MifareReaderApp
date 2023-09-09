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

            ViewModel.InitializePort();

            var operatorPageVM = (OperatorTab.Content as OperatorPage)!.ViewModel;
            ViewModel.InitializeViewModels(operatorPageVM);
        }
    }
}