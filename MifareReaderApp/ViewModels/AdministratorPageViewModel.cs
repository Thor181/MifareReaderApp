using MifareReaderApp.DataLogic;
using MifareReaderApp.Models;
using MifareReaderApp.Models.Stuff;
using MifareReaderApp.Stuff;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.ViewModels
{
    public class AdministratorPageViewModel : OperatorPageViewModel, INotifyPropertyChanged
    {
        public List<Place> Places { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private DbConnectionString _connectionString;
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

        public AdministratorPageViewModel()
        {
            FieldsIsEnabled = true;

            InitializeAvailableValues();
        }

        private void InitializeAvailableValues()
        {
            using var helperLogic = new HelperEntityLogic<Place>();
            Places = helperLogic.GetAll();
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
