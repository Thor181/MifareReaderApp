using Microsoft.VisualBasic;
using Microsoft.Win32;
using MifareReaderApp.DataLogic;
using MifareReaderApp.DataLogic.Stuff;
using MifareReaderApp.Models;
using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Models.Interfaces;
using MifareReaderApp.Models.Stuff;
using MifareReaderApp.Stuff;
using MifareReaderApp.Stuff.Commands;
using MifareReaderApp.Stuff.Results;
using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MifareReaderApp.ViewModels
{
    public class AdministratorPageViewModel : OperatorPageViewModel, INotifyPropertyChanged, INotifyCollectionChanged
    {

        private bool _pageIsEnabled = true;
        public bool PageIsEnabled
        {
            get { return _pageIsEnabled; }
            set { _pageIsEnabled = value; OnPropertyChanged(); }
        }

        public DataGrid TableDataGrid { get; set; }

        #region Properties
        private Visibility _progressVisibility = Visibility.Collapsed;
        public Visibility ProgressVisibility
        {
            get
            {
                return _progressVisibility;
            }
            set
            {
                _progressVisibility = value;
                OnPropertyChanged();
            }
        }

        public List<Place> Places { get; set; }

        public string PortName { get => AppConfig.Instance.PortName; set => AppConfig.Instance.PortName = value; }

        public DbConnectionString ConnectionString
        {
            get
            {
                return AppConfig.Instance.DbConnectionString;
            }
            set
            {
                AppConfig.Instance.DbConnectionString = value;
                OnPropertyChanged();
            }
        }

        public DateFilter DateFilter { get; set; } = new();
        #endregion

        #region Commands
        public SimpleCommand SaveDbConnectionStringCommand { get; set; }
        public SimpleCommand InitializeDatabaseCommand { get; set; }
        public SimpleCommand ChangeAdminPasswordCommand { get; set; }
        public SimpleCommand FilterTableValuesCommand { get; set; }
        public SimpleCommand ExportToExcelCommand { get; set; }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public Dictionary<string, Action<DateTime, DateTime>> AvailableTables { get; set; }

        private string _selectedTable;
        public string SelectedTable
        {
            get
            {
                return _selectedTable;
            }
            set
            {
                _selectedTable = value;

                AvailableTables[value].Invoke(DateFilter.DateFrom, DateFilter.DateTo);
            }
        }

        public ObservableCollection<AppliedUser> AppliedUsers { get; set; }
        public ObservableCollection<AppliedQREvent> AppliedQrEvents { get; set; }
        public ObservableCollection<AppliedCardEvent> AppliedCardEvents { get; set; }

        public AdministratorPageViewModel()
        {
            FieldsIsEnabled = true;

            InitializeCommands();

            InitializeAvailableValues();

            AppliedUsers = new();
            AppliedQrEvents = new();
            AppliedCardEvents = new();

            ConnectionString.OnConfigChange += AppConfig.Instance.Save;
        }

        private void InitializeCommands()
        {
            SaveDbConnectionStringCommand = new SimpleCommand()
            {
                CommandHandler = SaveDbConnectionString
            };

            InitializeDatabaseCommand = new SimpleCommand()
            {
                CommandHandler = InitializeDatabas
            };

            ChangeAdminPasswordCommand = new SimpleCommand()
            {
                CommandHandler = ChangeAdminPassword
            };

            FilterTableValuesCommand = new SimpleCommand()
            {
                CommandHandler = FilterTableValues
            };

            ExportToExcelCommand = new SimpleCommand()
            {
                CommandHandler = ExportToExcel
            };
        }

        private void SelectedTableValues_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(sender, e);
        }

        private void InitializeAvailableValues()
        {
            using var helperLogic = new HelperEntityLogic<Place>();
            Places = helperLogic.GetAll();

            AvailableTables = new Dictionary<string, Action<DateTime, DateTime>>
            {
                {"Пользователи (Users)", LoadUsers },
                {"QREvents", LoadQREvents },
                {"CardEvents", LoadCardEvents }
            };
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void SaveDbConnectionString(object? entity)
        {
            AppConfig.Instance.DbConnectionString = ConnectionString;
        }

        private void InitializeDatabas(object? parameter)
        {
            using var dbInitialization = new DbInitialization();
            var creationResult = dbInitialization.EnsureCreated();

            MessageDialog.ShowDialog(creationResult.Message);
        }

        private void ChangeAdminPassword(object? parameter)
        {
            var dialog = new InputPasswordDialog("Введите новый пароль", InputPasswordDialog.DialogAction.SetPassword);
            var result = dialog.ShowDialog();

            if (result == true)
                MessageDialog.ShowDialog($"Новый пароль успешно сохранен");
            else
                MessageDialog.ShowDialog($"Не удалось установить новый пароль");
        }

        public void OnTablesDataGridColumnGenerating(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            Type collectionType = dataGrid.ItemsSource.GetType();
            var itemType = collectionType.GetGenericArguments().Single();

            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy HH:mm";

            var name = MetadataInfo<IAppliedModel>.GetPropertyLocalizedName(itemType, e.PropertyName);

            if (MetadataInfo<IAppliedModel>.PropertyIsVirtual(itemType, e.PropertyName) && e.PropertyType != typeof(DateTime))
                e.Cancel = true;

            e.Column.Header = name;
        }

        private void LoadUsers(DateTime from, DateTime to)
        {
            using var logic = new UserLogic();

            var dbResult = logic.GetAllIncluded(x => x.Dt >= from && x.Dt <= to);
            if (!dbResult.IsSuccess)
            {
                MessageDialog.ShowDialog(dbResult.Message);
                return;
            }

            var dbValues = dbResult.Entity;

            var values = new List<User>();

            AppliedUsers.Clear();

            foreach (var item in dbValues)
                AppliedUsers.Add((AppliedUser)item);

            TableDataGrid.ItemsSource = AppliedUsers;
        }

        private void LoadQREvents(DateTime from, DateTime to)
        {
            using var logic = new QREventLogic();

            var dbResult = logic.GetAllIncluded(x => x.Dt >= from && x.Dt <= to);
            if (!dbResult.IsSuccess)
            {
                MessageDialog.ShowDialog(dbResult.Message);
                return;
            }

            var dbValues = dbResult.Entity;
            var values = new List<AppliedQREvent>();

            AppliedQrEvents.Clear();

            foreach (var item in dbValues)
                AppliedQrEvents.Add((AppliedQREvent)item);

            TableDataGrid.ItemsSource = AppliedQrEvents;
        }

        private void LoadCardEvents(DateTime from, DateTime to)
        {
            using var logic = new CardEventLogic();

            var dbResult = logic.GetAllIncluded(x => x.Dt >= from && x.Dt <= to);
            if (!dbResult.IsSuccess)
            {
                MessageDialog.ShowDialog(dbResult.Message);
                return;
            }

            var dbValues = dbResult.Entity;
            var values = new List<AppliedCardEvent>();

            AppliedCardEvents.Clear();

            foreach (var item in dbValues)
                AppliedCardEvents.Add((AppliedCardEvent)item);

            TableDataGrid.ItemsSource = AppliedCardEvents;
        }

        private void FilterTableValues(object? parameter)
        {
            AvailableTables[SelectedTable].Invoke(DateFilter.DateFrom, DateFilter.DateTo);
        }

        private void ExportToExcel(object? parameter)
        {
            var gridItems = TableDataGrid.Items;
            if (gridItems.Count == 0)
            {
                MessageDialog.ShowDialog("Нет записей для экспорта");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Файл Excel (*.xlsx,*.xls) | *.xlsx;*.xls;";

            if (saveFileDialog.ShowDialog() == true)
            {
                var d = TableDataGrid.ItemsSource.Cast<AppliedUser>();

                var headers = TableDataGrid.Columns.Select(x => x.Header.ToString()).ToList();

                var v = new List<List<string>>();

                foreach (var item in gridItems)
                {
                    var propertiesValues = MetadataInfo<IAppliedModel>.GetPropertiesValues((IAppliedModel)item);
                    v.Add(propertiesValues);
                }

                PageIsEnabled = false;
                Task.Run(async () =>
                {
                    var a = new Excel(headers, v, SelectedTable, saveFileDialog.FileName);
                    ProgressVisibility = Visibility.Visible;

                    var result = await a.Export();

                    MessageDialog.ShowDialog(result.Message);
                    ProgressVisibility = Visibility.Collapsed;
                    PageIsEnabled = true;
                });
            }


        }
    }
}
