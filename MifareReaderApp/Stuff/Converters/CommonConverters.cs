using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Converters
{
    public class CommonConverters
    {
        //9/17/2022 12:22 AM
        public static DateTime StringToDateTime(string datetime, string time)
        {
            var splittedDateTime = datetime.Split(' ');
            var splittedDate = splittedDateTime[0].Split('/');
            var day = Convert.ToInt32(splittedDate[1]);
            var month = Convert.ToInt32(splittedDate[0]);
            var year = Convert.ToInt32(splittedDate[2]);
            var splittedTime = time.Split(':');
            var hour = Convert.ToInt32(splittedTime[0]);
            var minute = Convert.ToInt32(splittedTime[1]);

            var date = new DateTime(year, month, day, hour, minute, 0);

            return date;
        }

    }
}
