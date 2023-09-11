using MifareReaderApp.DataLogic;
using MifareReaderApp.Models;
using MifareReaderApp.Stuff;
using MifareReaderApp.ViewModels;
using MifareReaderApp.Views;
using MifareReaderApp.Views.Controls.Status;
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

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AppConfig.Instance.Initialize();

            ViewModel.InitializePort();

            ViewModel.InitializeDatabase();

            var operatorPageVM = (OperatorTab.Content as OperatorPage)!.ViewModel;
            ViewModel.InitializeViewModels(operatorPageVM);

        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            

        }
    }
}