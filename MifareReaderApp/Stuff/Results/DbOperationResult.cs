using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MifareReaderApp.Stuff.Results
{
    public class DbOperationResult<T>  : BaseResult
    {
        public T? Entity { get; set; }
        public bool NotFound { get; set; }
    }
}
