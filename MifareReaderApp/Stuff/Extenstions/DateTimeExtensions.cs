using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Extenstions
{
    public static class DateTimeExtensions
    {
        public static DateTime EndDay(this DateTime d)
        {
            return d.AddDays(1).AddTicks(-1);
        }

        public static DateTime SqlMinValue(this DateTime d)
        {
            return DateTime.Parse("01.01.1800");
        }

        public static DateTime SqlMaxValue(this DateTime d)
        {
            return DateTime.Parse("31.12.9999");
        }

    }
}
