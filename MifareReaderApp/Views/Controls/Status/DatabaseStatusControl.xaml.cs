using MifareReaderApp.Stuff.Status;
using System;
using System.Collections.Generic;
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

namespace MifareReaderApp.Views.Controls.Status
{
    public partial class DatabaseStatusControl : UserControl
    {
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
            }
        }

        public SolidColorBrush StatusColor { get; set; }

        public DatabaseStatusControl()
        {
            ControlStatus = ControlStatus.RedStatus;

            InitializeComponent();
        }
    }
}
