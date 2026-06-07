using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Extenstions;
using MifareReaderApp.ViewModels;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views.Interfaces;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace MifareReaderApp.Views
{
    public partial class AdministratorPage : UserControl, IPage
    {
        public AdministratorPageViewModel ViewModel { get; set; }

        public AdministratorPage()
        {
            ViewModel = new AdministratorPageViewModel();

            InitializeComponent();

            ViewModel.TableDataGrid = this.TablesDataGrid;
        }

        public void BeforeOpen()
        {
            
        }

        private void TablesDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (sender is DataGrid dataGrid)
                dataGrid.OnDataGridColumnGenerating(e);
        }
    }
}
