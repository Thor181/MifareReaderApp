using MifareReaderApp.DataLogic;
using MifareReaderApp.Views.Dialogs;
using MifareReaderApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MifareReaderApp.Models;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using MifareReaderApp.Stuff.Commands;

namespace MifareReaderApp.ViewModels
{
    public class OperatorPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        private bool _fieldsIsEnabled;
        public bool FieldsIsEnabled
        {
            get { return _fieldsIsEnabled; }
            set { _fieldsIsEnabled = value; OnPropertyChanged(); }
        }

        private bool _buttonsIsEnabled;

        public bool ButtonsIsEnabled
        {
            get { return _buttonsIsEnabled; }
            set { _buttonsIsEnabled = value; OnPropertyChanged(); }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        #endregion

        public SaveUserDataCommand SaveCommand { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public OperatorPageViewModel()
        {
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void HandleUser(string cardNumber)
        {
            using var userLogic = new UserLogic();
            var result = userLogic.FindUser(cardNumber);

            if (!result.IsSuccess)
            {
                MessageDialog.ShowDialog(result.Message);
                return;
            }

            if (result.NotFound == true)
            {
                
                User = new User() { Card = cardNumber };
                FieldsIsEnabled = true;
                ButtonsIsEnabled = true;
                return;
            }
        }
    }
}
