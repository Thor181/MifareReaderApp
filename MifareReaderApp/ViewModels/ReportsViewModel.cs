using Microsoft.Win32;
using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Models.Stuff;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Commands;
using MifareReaderApp.Stuff.Extenstions;
using MifareReaderApp.Stuff.Reports;
using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MifareReaderApp.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        public Dictionary<string, IReport> AvailableReports { get; }

        public string? SelectedReport
        {
            get;
            set => SetProperty(ref field, value);
        }

        public DateFilter DateFilter
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool ProgressVisibility
        {
            get;
            set => SetProperty(ref field, value);
        }

        public bool PageIsEnabled
        {
            get;
            set => SetProperty(ref field, value);
        }

        public DataGrid ReportValuesDataGrid { get; set; }

        public ICommand GenerateReportCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }

        public ObservableCollection<object> SelectedReportValues { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReportsViewModel()
        {
            AvailableReports = new() {
                { "Использование карты", new CardUsageReport()  }
            };

            ProgressVisibility = false;
            PageIsEnabled = true;
            SelectedReportValues = [];
            DateFilter = new DateFilter();
            GenerateReportCommand = new SimpleCommand(GenerateReport);
            ExportToExcelCommand = new SimpleCommand(ExportToExcel);
        }

        private void GenerateReport(object? parameter)
        {
            if (string.IsNullOrEmpty(SelectedReport))
                return;

            try
            {
                ProgressVisibility = true;
                var report = AvailableReports[SelectedReport];

                var beforeExecuteResult = report.BeforeExecute();

                if (!beforeExecuteResult.IsSuccess)
                {
                    MessageDialog.ShowDialog(beforeExecuteResult.Message);
                    return;
                }

                var execueResult = report.Execute(SelectedReportValues, DateFilter);

                if (!execueResult.IsSuccess)
                {
                    MessageDialog.ShowDialog(execueResult.Message);
                    return;
                }
            }
            finally
            {
                ProgressVisibility = false;
            }
        }

        private void ExportToExcel(object? parameter)
        {
            var gridItems = ReportValuesDataGrid.Items;
            if (gridItems.Count == 0)
            {
                MessageDialog.ShowDialog("Нет записей для экспорта");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Файл Excel (*.xlsx,*.xls) | *.xlsx;*.xls;";

            if (saveFileDialog.ShowDialog() == true)
            {
                var headers = ReportValuesDataGrid.Columns.Select(x => x.Header.ToString()).ToList();

                var rows = new List<List<string>>();

                foreach (var item in gridItems)
                {
                    var propertiesValues = MetadataInfo<IAppliedModel>.GetPropertiesValues((IAppliedModel)item);
                    rows.Add(propertiesValues);
                }

                PageIsEnabled = false;
                Task.Run(async () =>
                {
                    var report = new Excel(headers, rows, SelectedReport, saveFileDialog.FileName);
                    ProgressVisibility = true;

                    var result = await report.Export();

                    MessageDialog.ShowDialog(result.Message);
                    ProgressVisibility = false;
                    PageIsEnabled = true;
                });
            }
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string prop = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
