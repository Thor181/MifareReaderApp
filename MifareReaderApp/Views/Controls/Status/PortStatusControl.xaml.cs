using MifareReaderApp.Stuff.Status;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvailableControlStatus = MifareReaderApp.Stuff.Status;

namespace MifareReaderApp.Views.Controls.Status
{
    public partial class PortStatusControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ControlStatus _controlStatus;
        public ControlStatus ControlStatus
        {
            get
            {
                return _controlStatus;
            }
            set
            {
                _controlStatus = value;
                StatusColor = (SolidColorBrush)App.Current.TryFindResource(value.ToString());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ControlStatus)));
            }
        }

        private SolidColorBrush _statusColor = (SolidColorBrush)App.Current.TryFindResource(AvailableControlStatus.ControlStatus.RedStatus.ToString());
        public SolidColorBrush StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                _statusColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusColor)));
            }
        }

        public PortStatusControl()
        {
            InitializeComponent();
        }
    }
}
