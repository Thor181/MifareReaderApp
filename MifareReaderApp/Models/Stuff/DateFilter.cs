using MifareReaderApp.Stuff.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Models.Stuff
{
    public class DateFilter
    {
        private DateTime _dateFrom = DateTime.Today;
        public DateTime DateFrom
        {
            get { return _dateFrom; }

            set
            {
                if (value < value.SqlMinValue())
                    value = value.SqlMinValue();

                _dateFrom = value;
            }
        }

        private DateTime _dateTo = DateTime.Today.EndDay();
        public DateTime DateTo
        {
            get { return _dateTo; }

            set
            {
                if (value > value.SqlMaxValue())
                    value = value.SqlMaxValue();

                _dateTo = value.EndDay();
            }
        }
    }
}
