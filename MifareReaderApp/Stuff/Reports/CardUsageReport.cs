using MifareReaderApp.DataLogic;
using MifareReaderApp.Models;
using MifareReaderApp.Models.AppliedModes;
using MifareReaderApp.Models.Stuff;
using MifareReaderApp.Stuff.Results;
using MifareReaderApp.Views.Dialogs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Reports
{
    public class CardUsageReport : IReport
    {
        private User? _selectedUser;

        public Type ReportEntityType => typeof(AppliedCardEvent);

        public CardUsageReport()
        {

        }

        public BaseResult BeforeExecute()
        {
            return SelectUser();
        }

        public BaseResult Execute(ICollection<object> reportValues, DateFilter dateFilter)
        {
            if (_selectedUser == null)
                return new BaseResult() { IsSuccess = false, Message = "Пользователь должен быть выбран перед выполнением отчета" };

            using var cardEventLogic = new CardEventLogic();
            var result = cardEventLogic.GetAllIncluded(x => x.Card == _selectedUser.Card && x.Dt >= dateFilter.DateFrom && x.Dt <= dateFilter.DateTo);

            if (!result.IsSuccess)
                return result;

            reportValues.Clear();
            foreach (var entity in result.Entity)
            {

                var appliedCardEvent = (AppliedCardEvent)entity;

                reportValues.Add(appliedCardEvent);
            }

            return new BaseResult() { IsSuccess = true, Message = string.Empty };
        }

        private BaseResult SelectUser()
        {
            using var userLogic = new UserLogic();
            var result = userLogic.GetAll<User>();

            if (!result.IsSuccess)
                return result;

            var dialog = new SelectItemDialog(result.Entity, nameof(User.FullNameWithCrad), "Выберите пользователя:");
            dialog.ShowDialog();

            if (dialog.DialogResult != true)
                return new BaseResult() { IsSuccess = true, Message = string.Empty };

            _selectedUser = dialog.SelectedItem as User;

            return new BaseResult() { IsSuccess = true, Message = string.Empty };
        }
    }
}
