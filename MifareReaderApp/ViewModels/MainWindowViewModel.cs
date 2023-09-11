using MifareReaderApp.DataLogic;
using MifareReaderApp.DataLogic.Stuff;
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
        private StatusControl PortStatusControl => MainWindow.PortStatusControl;
        private StatusControl DatabaseStatusControl => MainWindow.DatabaseStatusControl;
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

        private HealthLogic _healthLogic;

        public MainWindowViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        #region Port
        public void InitializePort()
        {
            PortWorker = new PortWorker();
            PortWorker.OnPortDataReceived += OnPortDataReceived;
            PortWorker.OnPortOpened += OnPortOpened;
        }

        private void OnPortOpened(bool result)
        {
            if (result == true)
                PortStatusControl.ControlStatus = Stuff.Status.ControlStatus.GreenStatus;
            else
                PortStatusControl.ControlStatus = Stuff.Status.ControlStatus.RedStatus;
        }

        private void OnPortDataReceived(string data)
        {
            var interpretationResult = DataInterpreter.GetCardNumber(data);

            if (!interpretationResult.IsSuccess)
            {
                App.DispatcherInvoke(() =>
                {
                    MessageDialog.ShowDialog(interpretationResult.Message);
                });
                return;
            }

            OperatorPage.HandleUser(interpretationResult.Message);
        }
        #endregion

        public void InitializeDatabase()
        {
            _healthLogic = new HealthLogic();
            _healthLogic.OnConnected += OnDatabaseConnected;
            _healthLogic.CheckDbAvailable();
        }

        private void OnDatabaseConnected(bool result)
        {
            if (result == true)
            {
                DatabaseStatusControl.ControlStatus = Stuff.Status.ControlStatus.GreenStatus;
                //_healthLogic.OnConnected -= OnDatabaseConnected;
                //_healthLogic.Dispose();
            }
            else
            {
                DatabaseStatusControl.ControlStatus = Stuff.Status.ControlStatus.RedStatus;

            }
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
    }
}
