using MifareReaderApp.Stuff;
using MifareReaderApp.ViewModels;
using MifareReaderApp.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MifareReaderApp.Views
{
    public partial class OperatorPage : UserControl, IPage
    {
        public OperatorPageViewModel ViewModel { get; set; }

        private static DateTime PersistentDate { get; set; }

        public OperatorPage()
        {
            ViewModel = new OperatorPageViewModel();
            InitializeComponent();
        }

        public void BeforeOpen()
        {

        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DatePick.SelectedDate == null)
                return;

            var date = DatePick.SelectedDate.Value.Date;
            if (date != PersistentDate)
            {
                TimePick.SelectedTime = new DateTime(date.Year, date.Month, date.Day, 14, 0, 0);
                PersistentDate = date;
            }

        }
    }
}
