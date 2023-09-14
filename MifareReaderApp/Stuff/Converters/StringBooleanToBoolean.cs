using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MifareReaderApp.Stuff.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class StringBooleanToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val == true ? "Да" : "Нет";
            }

            if (((ComboBoxItem)value).Content is string stringValue)
            {
                return stringValue == "Да";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((ComboBoxItem)value).Content is string stringValue)
            {
                return stringValue == "Да";
            }

            return null;
        }
    }
}
