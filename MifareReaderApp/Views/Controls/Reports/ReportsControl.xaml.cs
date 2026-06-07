using MifareReaderApp.Stuff.Extenstions;
using MifareReaderApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MifareReaderApp.Views.Controls.Reports
{
    public partial class ReportsControl : UserControl
    {
        private readonly ReportsViewModel _viewModel;

        public ReportsControl()
        {
            InitializeComponent();
            _viewModel = DataContext as ReportsViewModel;
            _viewModel.ReportValuesDataGrid = ReportValuesDataGrid;
        }

        private void TablesDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var reportEntityType = _viewModel.AvailableReports[_viewModel.SelectedReport].ReportEntityType;
                dataGrid.OnDataGridColumnGenerating(e, reportEntityType);
            }
        }
    }
}
