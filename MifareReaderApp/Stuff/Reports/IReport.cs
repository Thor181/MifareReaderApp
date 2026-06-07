using MifareReaderApp.Models.Stuff;
using MifareReaderApp.Stuff.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Reports
{
    public interface IReport
    {
        public Type ReportEntityType { get; }
        public BaseResult BeforeExecute();
        public BaseResult Execute(ICollection<object> reportValues, DateFilter dateFilter);
    }
}
