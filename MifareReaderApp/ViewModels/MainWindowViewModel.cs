using MifareReaderApp.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MifareReaderApp.ViewModels
{
    public class MainWindowViewModel
    {
        private TabItem? _selectedTab;
        public TabItem SelectedTab
        {
            get
            {
                return _selectedTab!;
            }
            set
            {
                _selectedTab = value;
                OnSelectedTabChanged(value);
            }
        }

        public MainWindowViewModel()
        {
        }

        private void OnSelectedTabChanged(TabItem selectedTab)
        {
            var page = selectedTab.Content as IPage;
            page?.BeforeOpen();
        }
    }

    
}
