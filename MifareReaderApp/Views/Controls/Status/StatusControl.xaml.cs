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
    public partial class StatusControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(nameof(ImageMargin), typeof(Thickness), typeof(StatusControl));
        public Thickness ImageMargin
        {
            get => (Thickness)this.GetValue(ImageMarginProperty);
            set => this.SetValue(ImageMarginProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof(ImageSource), typeof(string), typeof(StatusControl));
        public string ImageSource
        {
            get => this.GetValue(ImageSourceProperty) as string;
            set => this.SetValue(ImageSourceProperty, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private AvailableControlStatus.ControlStatus _controlStatus;
        public AvailableControlStatus.ControlStatus ControlStatus
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

        public StatusControl()
        {
            InitializeComponent();
        }
    }
}
