using MifareReaderApp.DataLogic;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Port;
using MifareReaderApp.Views;
using MifareReaderApp.Views.Controls.Status;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MifareReaderApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private MainWindow MainWindow { get; set; }
        private PortStatusControl PortStatusControl => MainWindow.PortStatusControl;
        private DatabaseStatusControl DatabaseStatusControl => MainWindow.DatabaseStatusControl;
        private OperatorPageViewModel OperatorPage;

        private TabItem? PreviousTab { get; set; }

        private TabItem? _selectedTab;

        public TabItem SelectedTab
        {
            get
            {
                return _selectedTab!;
            }
            set
            {
                PreviousTab = _selectedTab ?? value;
                _selectedTab = value;
                OnSelectedTabChanged(value);
            }
        }

        private PortWorker PortWorker { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public void InitializePort()
        {
            PortWorker = new PortWorker();
            PortWorker.OnPortDataReceived += OnPortDataReceived;
        }

        public void InitializeViewModels(OperatorPageViewModel operatorPageViewModel)
        {
            OperatorPage = operatorPageViewModel;
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void OnSelectedTabChanged(TabItem selectedTab)
        {
            var page = selectedTab.Content as IPage;
            page?.BeforeOpen();
        }

        private void OnPortDataReceived(string data)
        {
            var interpretationResult = DataInterpreter.GetCardNumber(data);

            if (!interpretationResult.IsSuccess)
            {
                MessageDialog.ShowDialog(interpretationResult.Message);
                return;
            }

            OperatorPage.HandleUser(interpretationResult.Message);


            

        }
    }


}
