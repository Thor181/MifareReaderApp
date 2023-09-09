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

namespace MifareReaderApp.ViewModels
{
    public class OperatorPageViewModel : INotifyPropertyChanged
    {
        public bool FieldsIsEnabled { get; set; } = false;
        public bool ButtonsIsEnabled { get; set; } = false;

        private string _cardNumber;
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                _cardNumber = value;
                OnPropertyChanged();
            }
        }

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
                OperatorPage.
            }
        }
    }
}
