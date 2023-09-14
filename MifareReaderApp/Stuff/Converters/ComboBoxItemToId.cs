using MifareReaderApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MifareReaderApp.Stuff.Converters
{
    [ValueConversion(typeof(Place), typeof(int))]
    public class ComboBoxItemToId : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertObject(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return ConvertObject(value);
        }

        private object ConvertObject(object value)
        {
            if (value is Place place)
            {
                return place.Id;
            }

            return value;
        }
    }
}
