using MifareReaderApp.DataLogic;
using MifareReaderApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.ViewModels
{
    public class AdministratorPageViewModel : OperatorPageViewModel
    {
        public List<Place> Places { get; set; }

        public AdministratorPageViewModel()
        {
            FieldsIsEnabled = true;

            InitializeAvailableValuse();
        }

        private void InitializeAvailableValuse()
        {
            using var helperLogic = new HelperEntityLogic<Place>();
            Places = helperLogic.GetAll();
        }
    }
}
