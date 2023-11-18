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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
        public int BaudRate { get => AppConfig.Instance.BaudRate; set => AppConfig.Instance.BaudRate = value; }

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
        public string DefaultPlace { get => AppConfig.Instance.DefaultPlace; set => AppConfig.Instance.DefaultPlace = value; }

        public string ID1Name { get => AppConfig.Instance.ID1Name; set => AppConfig.Instance.ID1Name = value; }
        public string ID2Name { get => AppConfig.Instance.ID2Name; set => AppConfig.Instance.ID2Name = value; }

        private bool _isSearchVisible = false;
        public bool IsSearchVisible
        {
            get
            {
                return _isSearchVisible;
            }
            set
            {
                _isSearchVisible = value; OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public SimpleCommand SaveDbConnectionStringCommand { get; set; }
        public SimpleCommand InitializeDatabaseCommand { get; set; }
        public SimpleCommand ChangeAdminPasswordCommand { get; set; }
        public SimpleCommand FilterTableValuesCommand { get; set; }
        public SimpleCommand ExportToExcelCommand { get; set; }
        public SimpleCommand SearchCommand { get; set; }
        public SimpleCommand EditCommand { get; set; }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public Dictionary<string, Action<DateTime, DateTime, string>> AvailableTables { get; set; }

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

                AvailableTables[value].Invoke(DateFilter.DateFrom, DateFilter.DateTo, null);
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
                CommandHandler = InitializeDatabase
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

            SearchCommand = new SimpleCommand()
            {
                CommandHandler = SearchUser
            };
            
            EditCommand = new SimpleCommand()
            {
                CommandHandler = EditUser
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

            AvailableTables = new()
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

        private void InitializeDatabase(object? parameter)
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

        private void LoadUsers(DateTime from, DateTime to, string searchString)
        {
            using var logic = new UserLogic();

            DbOperationResult<List<User>> dbResult;
            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearchString = searchString.ToLower();

                Expression<Func<User, bool>> filterPredicate = (User x) =>
                x.Name.ToLower().Contains(lowerSearchString)
                || x.Name2.ToLower().Contains(lowerSearchString)
                || x.Surname.ToLower().Contains(lowerSearchString)
                || x.Card.ToLower().Contains(lowerSearchString)
                || x.Id1.ToLower().Contains(lowerSearchString)
                || x.Id2.ToLower().Contains(lowerSearchString)
                || x.Place.Name.ToLower().Contains(lowerSearchString)
                && (x.Dt >= from && x.Dt <= to);
                dbResult = logic.GetAllIncluded(filterPredicate);
            }
            else
            {
                dbResult = logic.GetAllIncluded(x => x.Dt >= from && x.Dt <= to);
            }
            
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
            IsSearchVisible = true;
        }

        private void LoadQREvents(DateTime from, DateTime to, string searchString)
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
            IsSearchVisible = false;
        }

        private void LoadCardEvents(DateTime from, DateTime to, string searchString)
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
            IsSearchVisible = false;
        }

        private void FilterTableValues(object? parameter)
        {
            AvailableTables[SelectedTable].Invoke(DateFilter.DateFrom, DateFilter.DateTo, null);
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

        private void SearchUser(object? parameter)
        {
            var dialog = new InputDialog("Введите поисковый запрос");
            var request = dialog.ShowDialog();
            LoadUsers(DateFilter.DateFrom, DateFilter.DateTo, request);
        }

        private void EditUser(object? parameter)
        {
            var card = (parameter as AppliedUser)?.Card;
            if (!string.IsNullOrEmpty(card))
            {
                MainWindowViewModel.OnPortDataReceivedInternal?.Invoke($"[{card}]");
                
                MessageDialog.ShowDialog($"Запись с номером карты \"{card}\" выбрана для редактирования.\n\nПерейдите на вкладку \"Оператор\" для продолжения.");
            }
            else
            {
                MessageDialog.ShowDialog("У выбранной записи не обнаружен номер карты");
            }
        }
    }
}
