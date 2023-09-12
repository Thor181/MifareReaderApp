using MifareReaderApp.DataLogic;
using MifareReaderApp.Models;
using MifareReaderApp.Stuff.Constants;
using MifareReaderApp.Stuff.Converters;
using MifareReaderApp.Stuff.Extenstions;
using MifareReaderApp.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MifareReaderApp.Stuff.Commands
{
    public class SaveUserDataCommand : SimpleCommand
    {
        public override void Execute(object? parameter)
        {
            var user = parameter as User;
            if (user != null)
            {
                user.Before = CommonConverters.StringToDateTime(user.BeforeDate, user.BeforeTime);

                if (user.Before < DateTime.Now)
                {
                    MessageDialog.ShowDialog($"Срок действия {user.Before} не может быть раньше текущей даты");
                    return;
                }

                var localizedValues = new (string PropName, string PropValue)[]
                {
                    user.Name.GetLocalizedPropInfo<User>(x => x.Name),
                    user.Name2.GetLocalizedPropInfo<User>(x => x.Name2),
                    user.Surname.GetLocalizedPropInfo<User>(x => x.Surname),
                    user.Id1.GetLocalizedPropInfo<User>(x => x.Id1),
                    user.Id2.GetLocalizedPropInfo<User>(x => x.Id2),
                    user.Card.GetLocalizedPropInfo<User>(x => x.Card)
                };

                var validationResult = Validation.ValidateStrings(localizedValues);

                if (!validationResult.IsSuccess)
                {
                    MessageDialog.ShowDialog($"Следующие поля не могут пустыми:\n{string.Join('\n', validationResult.NotValidEntities)}");
                    return;
                }

                using var userLogic = new UserLogic();
                var result = userLogic.AddUser(user);

                if (!result.IsSuccess)
                {
                    MessageDialog.ShowDialog($"Во время добавления сущности {nameof(User)} возникла ошибка\n{result.Message}");
                    return;
                }

                MessageDialog.ShowDialog(result.Message);
            }
        }
    }
}
