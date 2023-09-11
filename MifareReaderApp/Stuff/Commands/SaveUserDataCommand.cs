using MifareReaderApp.Models;
using MifareReaderApp.Stuff.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            }
        }
    }
}
